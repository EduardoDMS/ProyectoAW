using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Guias.DTOs.Responses;
using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Maestros.Estados.Entities;
using ProyectoAW.Modules.Maestros.Operaciones.Entities;
using ProyectoAW.Modules.Seguridad.Usuarios.Entitites;
using ProyectoAW.Modules.Stocks.Dtos.Request;
using ProyectoAW.Modules.Stocks.Dtos.Response;
using ProyectoAW.Modules.Stocks.Entities;
using ProyectoAW.Modules.Stocks.Repositories;
using ProyectoAW.Modules.Stocks.Validacion;
using ProyectoAW.Modules.Seguridad.Security_Session.Permisos.HttpAuth;

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

        public MovimientoStockService(ValidacionesGuia validacionesGuia,
                                      ValidacionesUbicacion validacionesUbicacion,
                                      ValidacionesStock validacionesStock,
                                      IMovimientoStockRepository movimientoStockRepository,
                                      ITransactionManager transactions,
                                      ICurrentUserService currentUser,
                                      ILecturaGuiaRepository repository)
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
            await _transactions.ExecuteSerializableAsync(_ => FinalizarLecturaEnTransaccionAsync(idGuia));
        }

        public async Task<IReadOnlyList<MovimientoStockDto>> ListarAsync(int? idGuia, int? idAlmacen, int? idOperacion)
        {
            var movimientos = await _movimientoStockRepo.ListarMovimientosStockAsync(idGuia, idAlmacen, idOperacion);
            return movimientos.Select(movimiento => new MovimientoStockDto
            {
                IdMovimientoStock = movimiento.IdMovimientoStock,
                IdProducto = movimiento.IdProducto,
                IdAlmacen = movimiento.IdAlmacen,
                IdTipoOperacion = movimiento.IdTipoOperacion,
                Cantidad = movimiento.Cantidad,
                FchMovimiento = movimiento.FchMovimiento,
                IdGuia = movimiento.IdGuia,
                Observacion = movimiento.Observacion,
                CantidadDiferenciada = movimiento.CantidadDiferenciada,
                IdUbicacionOrigen = movimiento.IdUbicacionOrigen,
                IdUbicacionDestino = movimiento.IdUbicacionDestino,
                NumeroGuia = movimiento.Guia?.NumeroGuia ?? string.Empty,
                MotivoGuia = movimiento.Guia?.Motivo?.NombreMotivo ?? "—",
                Operacion = movimiento.OperacionGuias?.Nombre ?? string.Empty,
                CodigoProducto = movimiento.Producto?.Codigo ?? string.Empty,
                Producto = movimiento.Producto?.Nombre ?? string.Empty,
                Origen = NombreOrigen(movimiento),
                Destino = NombreDestino(movimiento),
                CantidadEsperada = movimiento.Guia?.Detalles
                    .FirstOrDefault(detalle => detalle.IdProducto == movimiento.IdProducto)?.Cantidad ?? 0,
                IdUsuarioOperador = movimiento.IdUsuarioOperador,
                Operador = NombreOperador(movimiento.Usuario),
                UbicacionOrigen = movimiento.UbicacionOrigen?.Codigo ?? string.Empty,
                UbicacionDestino = movimiento.UbicacionDestino?.Codigo ?? string.Empty
            }).ToList();
        }

        public async Task<IReadOnlyList<GuiaOperadorDto>> ListarGuiasOperadorAsync(int? idEstadoGuia)
        {
          //  await _validacionesGuia.ExigirOperadorAsync();

            var guia = await _repository.ListarGuiasParaOperadorAsync(_currentUser.IdUsuario, idEstadoGuia);
            return guia.Select(item =>
            {
                var requiereProveedor = item.Motivo?.RequiereProveedor ??
                    item.IdTipoOperacionGuia == TipoOperacionGuiaIds.Entrada;
                var requiereCliente = item.Motivo?.RequiereCliente ??
                    item.IdTipoOperacionGuia == TipoOperacionGuiaIds.Salida;
                var requiereAlmacenOrigen = item.Motivo?.RequiereAlmacenOrigen ??
                    item.IdTipoOperacionGuia == TipoOperacionGuiaIds.Salida;
                var requiereAlmacenDestino = item.Motivo?.RequiereAlmacenDestino ??
                    item.IdTipoOperacionGuia == TipoOperacionGuiaIds.Entrada;

                return new GuiaOperadorDto
                {
                    IdGuia = item.IdGuia,
                    NumeroGuia = item.NumeroGuia,
                    IdTipoOperacionGuia = item.IdTipoOperacionGuia,
                    IdMotivoGuia = item.IdMotivoGuia,
                    IdTipoComportamiento = item.Motivo?.IdTipoComportamiento,
                    NombreMotivo = item.Motivo?.NombreMotivo,
                    RequiereProveedor = requiereProveedor,
                    RequiereCliente = requiereCliente,
                    RequiereAlmacenOrigen = requiereAlmacenOrigen,
                    RequiereAlmacenDestino = requiereAlmacenDestino,
                    TipoTercero = requiereProveedor ? "Proveedor" : requiereCliente ? "Cliente" : null,
                    NombreTercero = requiereProveedor
                        ? item.Proveedor?.RazonSocial
                        : requiereCliente ? item.Cliente?.RazonSocial : null,
                    NombreAlmacenOrigen = item.AlmacenOrigen is null
                        ? null
                        : string.Join(" · ", new[] { item.AlmacenOrigen.Codigo, item.AlmacenOrigen.Nombre }
                            .Where(valor => !string.IsNullOrWhiteSpace(valor))),
                    NombreAlmacenDestino = item.AlmacenDestino is null
                        ? null
                        : string.Join(" · ", new[] { item.AlmacenDestino.Codigo, item.AlmacenDestino.Nombre }
                            .Where(valor => !string.IsNullOrWhiteSpace(valor))),
                    IdGuiaRelacionada = item.IdGuiaRelacionada,
                    NumeroGuiaRelacionada = item.GuiaRelacionada?.NumeroGuia,
                    IdEstadoGuia = item.IdEstadoGuia,
                    IdProveedor = item.IdProveedor,
                    IdCliente = item.IdCliente,
                    IdAlmacenOrigen = item.IdAlmacenOrigen,
                    IdAlmacenDestino = item.IdAlmacenDestino,
                    FechaProgramada = item.FechaProgramada,
                    FechaRegistro = item.FechaRegistro,
                    FechaActualizada = item.FechaActualizada,
                    TipoDocumentoReferencia = item.TipoDocumentoReferencia,
                    NumeroDocumentoReferencia = item.NumeroDocumentoReferencia,
                    Observacion = item.Observacion,
                    Detalles = item.Detalles.Select(detalle => new DetalleGuiaResponse
                    {
                        IdGuiaDetalle = detalle.IdDetalleGuia,
                        IdProducto = detalle.IdProducto,
                        CantidadEsperada = detalle.Cantidad,
                        CodProducto = detalle.CodProducto
                    }).ToList()
                };
            }).ToList();
            
        }

        public async Task<GuiaOperadorDto> ObtenerMiGuiaAsync(int idGuia)
        {
          //  await _validacionesGuia.ExigirOperadorAsync();
            var guia = await _repository.ObtenerGuiaOperadorAsync(idGuia, _currentUser.IdUsuario)
                 ?? throw new NegocioException("La guia no existe en tus guias en proceso o en tus guias terminadas. . .");

            ValidacionesGuia.ExigirOperacionesOperador(guia.IdTipoOperacionGuia);
            var requiereProveedor = guia.Motivo?.RequiereProveedor ??
                guia.IdTipoOperacionGuia == TipoOperacionGuiaIds.Entrada;
            var requiereCliente = guia.Motivo?.RequiereCliente ??
                guia.IdTipoOperacionGuia == TipoOperacionGuiaIds.Salida;
            var requiereAlmacenOrigen = guia.Motivo?.RequiereAlmacenOrigen ??
                guia.IdTipoOperacionGuia == TipoOperacionGuiaIds.Salida;
            var requiereAlmacenDestino = guia.Motivo?.RequiereAlmacenDestino ??
                guia.IdTipoOperacionGuia == TipoOperacionGuiaIds.Entrada;

            return new GuiaOperadorDto
            {
                IdGuia = guia.IdGuia,
                NumeroGuia = guia.NumeroGuia,
                IdTipoOperacionGuia = guia.IdTipoOperacionGuia,
                IdMotivoGuia = guia.IdMotivoGuia,
                IdTipoComportamiento = guia.Motivo?.IdTipoComportamiento,
                NombreMotivo = guia.Motivo?.NombreMotivo,
                RequiereProveedor = requiereProveedor,
                RequiereCliente = requiereCliente,
                RequiereAlmacenOrigen = requiereAlmacenOrigen,
                RequiereAlmacenDestino = requiereAlmacenDestino,
                TipoTercero = requiereProveedor ? "Proveedor" : requiereCliente ? "Cliente" : null,
                NombreTercero = requiereProveedor
                    ? guia.Proveedor?.RazonSocial
                    : requiereCliente ? guia.Cliente?.RazonSocial : null,
                NombreAlmacenOrigen = guia.AlmacenOrigen is null
                    ? null
                    : string.Join(" · ", new[] { guia.AlmacenOrigen.Codigo, guia.AlmacenOrigen.Nombre }
                        .Where(valor => !string.IsNullOrWhiteSpace(valor))),
                NombreAlmacenDestino = guia.AlmacenDestino is null
                    ? null
                    : string.Join(" · ", new[] { guia.AlmacenDestino.Codigo, guia.AlmacenDestino.Nombre }
                        .Where(valor => !string.IsNullOrWhiteSpace(valor))),
                IdGuiaRelacionada = guia.IdGuiaRelacionada,
                NumeroGuiaRelacionada = guia.GuiaRelacionada?.NumeroGuia,
                IdEstadoGuia = guia.IdEstadoGuia,
                IdProveedor = guia.IdProveedor,
                IdCliente = guia.IdCliente,
                IdAlmacenOrigen = guia.IdAlmacenOrigen,
                IdAlmacenDestino = guia.IdAlmacenDestino,
                FechaProgramada = guia.FechaProgramada,
                FechaRegistro = guia.FechaRegistro,
                FechaActualizada = guia.FechaActualizada,
                TipoDocumentoReferencia = guia.TipoDocumentoReferencia,
                NumeroDocumentoReferencia = guia.NumeroDocumentoReferencia,
                Observacion = guia.Observacion,
                Detalles = guia.Detalles.Select(detalle => new DetalleGuiaResponse
                {
                    IdGuiaDetalle = detalle.IdDetalleGuia,
                    IdProducto = detalle.IdProducto,
                    CantidadEsperada = detalle.Cantidad
                }).ToList()
            };
        }

        public async Task<LecturaGuiaDto> RegistrarLecturaAsync(int idGuia, RegistrarLecturaGuiaRequest request)
        {
           // await _validacionesGuia.ExigirOperadorAsync();
            var lectura = await RegistrarLecturaEnTransaccionAsync(idGuia, request);
            return new LecturaGuiaDto
            {
                IdLecturaGuia = lectura.IdLecturaGuia,
                IdGuia = lectura.IdGuia,
                IdProducto = lectura.IdProducto,
                CodigoProducto = lectura.Producto?.Codigo ?? lectura.CodigoLeido,
                DescripcionProducto = lectura.Producto?.Nombre ?? string.Empty,
                CodigoLeido = lectura.CodigoLeido,
                Cantidad = lectura.Cantidad,
                Lote = lectura.Lote,
                Serie = lectura.Serie,
                Observacion = lectura.Observacion,
                IdUbicacionOrigen = lectura.IdUbicacionOrigen,
                UbicacionOrigen = lectura.UbicacionOrigen?.Codigo,
                IdUbicacionDestino = lectura.IdUbicacionDestino,
                UbicacionDestino = lectura.UbicacionDestino?.Codigo,
                FechaRegistroUtc = lectura.FechaRegistroUtc
            };
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
                guia.FechaActualizada = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);
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
            guia.FechaActualizada = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);

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




        private MovimientoStock CrearMovimiento(
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
                FchMovimiento = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now),
                IdUbicacionOrigen = ObtenerUbicacionComun(lecturas.Select(lectura => lectura.IdUbicacionOrigen)),
                IdUbicacionDestino = ObtenerUbicacionComun(lecturas.Select(lectura => lectura.IdUbicacionDestino))
            };
        }

        private static string NombreOperador(Usuario? usuario) =>
                                                      usuario is null ? string.Empty : $"{usuario.Nombre} {usuario.Apellido}".Trim();

        private static string NombreOrigen(MovimientoStock movimiento) =>
                                                           movimiento.IdTipoOperacion == TipoOperacionGuiaIds.Entrada
                                                           ? movimiento.Guia?.Proveedor?.RazonSocial ?? "Cliente"
                                                           : movimiento.Guia?.AlmacenDestino?.Nombre ?? "Almacen";

        private static string NombreDestino(MovimientoStock movimiento) =>
                                                            movimiento.IdTipoOperacion == TipoOperacionGuiaIds.Salida
                                                            ? movimiento.Guia?.Proveedor?.RazonSocial ?? "Cliente"
                                                            : movimiento.Guia?.AlmacenDestino?.Nombre ?? "Almacen";

        private static int? ObtenerUbicacionComun(IEnumerable<int?> ubicaciones)
        {
            var distintas = ubicaciones.Distinct().ToList();
            return distintas.Count == 1 ? distintas[0] : null;
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

                lectura = new LecturaGuia
                {
                    IdGuia = idGuia,
                    IdProducto = request.IdProducto,
                    IdUsuarioOperador = _currentUser.IdUsuario,
                    Cantidad = request.Cantidad,
                    CodigoLeido = request.CodigoLeido.Trim(),
                    Lote = string.IsNullOrWhiteSpace(request.Lote) ? null : request.Lote.Trim(),
                    Serie = string.IsNullOrWhiteSpace(request.Serie) ? null : request.Serie.Trim(),
                    Observacion = string.IsNullOrWhiteSpace(request.Observacion) ? null : request.Observacion.Trim(),
                    IdUbicacionOrigen = request.IdUbicacionOrigen is > 0 ? request.IdUbicacionOrigen : null,
                    IdUbicacionDestino = request.IdUbicacionDestino is > 0 ? request.IdUbicacionDestino : null,
                    FechaRegistroUtc = DateTimeText.From(DateTime.Now)
                };

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
            var movimiento = CrearMovimiento(
                guia, detalle, lecturasDelProducto, processor, idOperador,
                (int)totalLeido, cantidadEsperada);
            _movimientoStockRepo.AgregarMovimiento(movimiento);
        }

        #endregion


    }
}


