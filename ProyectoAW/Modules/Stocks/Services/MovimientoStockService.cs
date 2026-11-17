using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Maestros.Estados.Entities;
using ProyectoAW.Modules.Maestros.Operaciones.Entities;
using ProyectoAW.Modules.Stocks.Dtos.Request;
using ProyectoAW.Modules.Stocks.Dtos.Response;
using ProyectoAW.Modules.Stocks.Entities;
using ProyectoAW.Modules.Stocks.Factories;
using ProyectoAW.Modules.Stocks.Repositories;
using ProyectoAW.Modules.Stocks.Validacion;
using ProyectoAW.Modules.Seguridad.Permisos.HttpAuth;

namespace ProyectoAW.Modules.Stocks.Services
{
    public class MovimientoStockService : IMovimientoStockService
    {
        private readonly ValidacionesGuia _validacionesGuia;
        private readonly ValidacionesUbicacion _validacionesUbicacion;
        private readonly ValidacionesStock _validacionesStock;
        private readonly ILecturaGuiaRepository _repository;
        private readonly IMovimientoStockRepository _movimientoStockRepo;
        private readonly ITransactionManager _transactions;
        private readonly ICurrentUserService _currentUser;

        public MovimientoStockService(
            ValidacionesGuia validacionesGuia,
            ValidacionesUbicacion validacionesUbicacion,
            ValidacionesStock validacionesStock,
            IMovimientoStockRepository movimientoStockRepository,
            ITransactionManager transactions,
            ICurrentUserService currentUser,
            ILecturaGuiaRepository repository
            )
        {
            _validacionesGuia = validacionesGuia;
            _validacionesUbicacion = validacionesUbicacion;
            _validacionesStock = validacionesStock;
            _movimientoStockRepo = movimientoStockRepository;
            _repository = repository;
            _transactions = transactions;
            _currentUser = currentUser;
        }

        public async Task<bool> EliminarLecturaAsync(int idGuia, long idLecturaGuia)
        {
            await _validacionesGuia.ExigirOperadorAsync();

            var eliminada = false;

            // La guía, la lectura y su eliminación se procesan juntas para evitar cambios
            // si la guía deja de estar asignada o cambia de estado durante la operación.
            await _transactions.ExecuteSerializableAsync(async _ =>
            {
                var guia = await _validacionesGuia.ObtenerGuiaAsync(idGuia);
                ValidacionesGuia.ValidarGuiaEnProceso(guia, _currentUser.IdUsuario);

                var lectura = await _repository.ObtenerLecturaGuiaAsync(
                    idGuia, idLecturaGuia, _currentUser.IdUsuario);
                if (lectura is null)
                    return;

                _repository.EliminarLectura(lectura);
                await _repository.GuardarCambiosAsync();
                eliminada = true;
            });

            return eliminada;
        }

        public async Task FinalizarLecturaAsync(int idGuia)
        {
          //  await _validacionesGuia.ExigirOperadorAsync();
            await _transactions.ExecuteSerializableAsync(_ => FinalizarLecturaEnTransaccionAsync(idGuia));
        }

        public async Task<IReadOnlyList<MovimientoStockDto>> ListarAsync(int? idGuia, int? idAlmacen, int? idOperacion)
        {
            var movimientos = await _movimientoStockRepo.ListarMovimientosStockAsync(idGuia, idAlmacen, idOperacion);
            return movimientos.Select(GuiaMapper.ToMovimientoDto).ToList();
        }

        public async Task<IReadOnlyList<GuiaOperadorDto>> ListarGuiasOperadorAsync(int? idEstadoGuia)
        {
          //  await _validacionesGuia.ExigirOperadorAsync();

            var guia = await _repository.ListarGuiasParaOperadorAsync(_currentUser.IdUsuario, idEstadoGuia);
            return guia.Select(GuiaMapper.ToGuiaOperadorDto).ToList();
            
        }

        public async Task<GuiaOperadorDto> ObtenerMiGuiaAsync(int idGuia)
        {
          //  await _validacionesGuia.ExigirOperadorAsync();
            var guia = await _repository.ObtenerGuiaOperadorAsync(idGuia, _currentUser.IdUsuario)
                 ?? throw new NegocioException("La guia no existe en tus guias en proceso o en tus guias terminadas. . .");

            ValidacionesGuia.ExigirOperacionesOperador(guia.IdTipoOperacionGuia);
            return GuiaMapper.ToGuiaOperadorDto(guia);
        }

        public async Task<LecturaGuiaDto> RegistrarLecturaAsync(int idGuia, RegistrarLecturaGuiaRequest request)
        {
           // await _validacionesGuia.ExigirOperadorAsync();
            var lectura = await RegistrarLecturaEnTransaccionAsync(idGuia, request);
            return GuiaMapper.ToLecturaDto(lectura);
        }

        public async Task TomarGuiaAsync(int idGuia)
        {
            //await _validacionesGuia.ExigirOperadorAsync();
            await _transactions.ExecuteSerializableAsync(async _ =>
            {
                var guia = await _validacionesGuia.ObtenerGuiaAsync(idGuia);
                ValidacionesGuia.ValidarGuiaPuedeSerTomada(guia);

                guia.IdEstadoGuia = EstadoGuiasIds.EnProceso;
                guia.IdUsuarioModificacion = _currentUser.IdUsuario;
                guia.FechaActualizada = DateTime.UtcNow;
                await _repository.GuardarCambiosAsync();
            });

        }


        #region metodos de transaccion con contacto directo con service


        public async Task FinalizarLecturaEnTransaccionAsync(int idGuia)
        {
            var guia = await _validacionesGuia.ObtenerGuiaAsync(idGuia);
            var processor = _validacionesGuia.ValidarGuiaParaFinalizar(guia);

            var lecturas = await ObtenerLecturasValidadasAsync(guia, processor);

            await AplicarLecturasAsync(guia, lecturas, processor, _currentUser.IdUsuario);

            // Marca la guía como atendida únicamente después de preparar el stock y el historial.
            guia.IdEstadoGuia = EstadoGuiasIds.Atendida;
            guia.IdUsuarioModificacion = _currentUser.IdUsuario;
            guia.FechaActualizada = DateTime.UtcNow;

            await _repository.GuardarCambiosAsync();
        }
        public async Task<IReadOnlyList<LecturaGuia>> ObtenerLecturasValidadasAsync(
                Guia guia, IGuiaStockProcessor processor)
        {
            var lecturas = await _movimientoStockRepo.ObtenerLecturasParaFinalizarAsync(guia.IdGuia);
            var productosRequeridos = guia.Detalles.Select(detalle => detalle.IdProducto).ToHashSet();
            var productosLeidos = lecturas.Select(lectura => lectura.IdProducto).ToHashSet();
            if (!productosRequeridos.SetEquals(productosLeidos))
                throw new NegocioException("Registra al menos una lectura para cada producto de la guía antes de finalizar.");

            // La guía de salida exige ubicación de despacho si el almacén tiene alguna configurada.
            var requiereUbicacionDespacho = guia.IdTipoOperacionGuia == TipoOperacionGuiaIds.Salida &&
                await _movimientoStockRepo.TieneUbicacionesDespachoActivasAsync(guia.IdAlmacenOrigen!.Value);


            await _validacionesUbicacion.ValidarUbicacionesFinalesAsync(guia, lecturas, processor, requiereUbicacionDespacho);
            return lecturas;
        }




        private static MovimientoStock CrearMovimiento(
            Guia guia,
            DetalleGuia detalle,
            IReadOnlyList<LecturaGuia> lecturas,
            IGuiaStockProcessor processor,
            int idOperador,
            int totalLeido,
            int cantidadEsperada)
        {
            var observacion = lecturas
                .Select(lectura => lectura.Observacion)
                .FirstOrDefault(texto => !string.IsNullOrWhiteSpace(texto)) ?? string.Empty;

            return new MovimientoStock
            {
                IdProducto = detalle.IdProducto,
                IdAlmacen = processor.ObtenerAlmacenMovimiento(guia),
                IdTipoOperacion = processor.IdTipoOperacionGuia,
                IdGuia = guia.IdGuia,
                IdUsuarioOperador = idOperador,
                Cantidad = totalLeido,
                CantidadDiferenciada = checked(totalLeido - cantidadEsperada),
                Observacion = observacion,
                FchMovimiento = DateTime.UtcNow,
                IdUbicacionOrigen = MovimientosStockFactory.ObtenerUbicacionComun(lecturas.Select(lectura => lectura.IdUbicacionOrigen)),
                IdUbicacionDestino = MovimientosStockFactory.ObtenerUbicacionComun(lecturas.Select(lectura => lectura.IdUbicacionDestino))
            };
        }

        private async Task<LecturaGuia> RegistrarLecturaEnTransaccionAsync(
            int idGuia, RegistrarLecturaGuiaRequest request)
        {
            LecturaGuia? lectura = null;

            await _transactions.ExecuteSerializableAsync(async _ =>
            {
                var guia = await _validacionesGuia.ObtenerGuiaAsync(idGuia);
                ValidacionesGuia.ValidarGuiaEnProceso(guia, _currentUser.IdUsuario);
                await _validacionesStock.ValidarProductoLeidoAsync(guia, request);
                await _validacionesUbicacion.ValidarUbicacionesYDisponibilidadAsync(guia, idGuia, request);


                lectura = LecturaGuiaFactory.Crear(idGuia, _currentUser.IdUsuario, request);

                _repository.AgregarLectura(lectura);
                await _repository.GuardarCambiosAsync();
            });

            return lectura ?? throw new NegocioException("La transaccion termino sin registrar la lectura");
        }


        public async Task AplicarLecturasAsync(
        Guia guia,
        IReadOnlyList<LecturaGuia> lecturas,
        IGuiaStockProcessor processor,
        int idOperador)
        {
            // El procesador cambia los saldos; el servicio registra el historial común del movimiento.
            foreach (var detalle in guia.Detalles)
                await AplicarLecturasDelProductoAsync(guia, detalle, lecturas, processor, idOperador);
        }


        private async Task AplicarLecturasDelProductoAsync(
        Guia guia,
        DetalleGuia detalle,
        IReadOnlyList<LecturaGuia> lecturas,
        IGuiaStockProcessor processor,
        int idOperador)
        {
            var lecturasDelProducto = lecturas
                .Where(lectura => lectura.IdProducto == detalle.IdProducto)
                .ToList();
            var totalLeido = lecturasDelProducto.Sum(lectura => (long)lectura.Cantidad);
            if (totalLeido > int.MaxValue)
                throw new NegocioException("La suma leída supera el máximo permitido por el inventario.");

            await processor.AplicarProductoAsync(guia, detalle, lecturasDelProducto);

            var cantidadEsperada = checked((int)detalle.Cantidad);
            var movimiento = MovimientosStockFactory.CrearMovimiento(
                guia, detalle, lecturasDelProducto, processor, idOperador,
                (int)totalLeido, cantidadEsperada);
            _movimientoStockRepo.AgregarMovimiento(movimiento);
        }

        #endregion


    }
}


