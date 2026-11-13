using ProyectoAW.Modules.Stocks.Services;
using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Maestros.Estados.Entities;
using ProyectoAW.Modules.Seguridad.Security.HttpAuth;
using ProyectoAW.Modules.Stocks.Dtos.Request;
using ProyectoAW.Modules.Stocks.Dtos.Response;
using ProyectoAW.Modules.Stocks.Entities;
using ProyectoAW.Modules.Stocks.Factories;
using ProyectoAW.Modules.Stocks.Repositories;
using ProyectoAW.Modules.Stocks.Validacion;

namespace ProyectoAW.Modules.Stocks.Services
{
    public class MovimientoStockService : IMovimientoStockService
    {
        private readonly ValidacionesGuia _validacionesGuia;
        private readonly ValidacionesUbicacion _validacionesUbicacion;
        private readonly ValidacionesStock _validacionesStock;
        private readonly ILecturaGuiaRepository _repository;
        private readonly ITransactionManager _transactions;
        private readonly ICurrentUserService _currentUser;

        public MovimientoStockService(
            ValidacionesGuia validacionesGuia,
            ValidacionesUbicacion validacionesUbicacion,
            ValidacionesStock validacionesStock,
            ILecturaGuiaRepository repository,
            ITransactionManager transactions,
            ICurrentUserService currentUser)
        {
            _validacionesGuia = validacionesGuia;
            _validacionesUbicacion = validacionesUbicacion;
            _validacionesStock = validacionesStock;
            _repository = repository;
            _transactions = transactions;
            _currentUser = currentUser;
        }

        public Task<bool> EliminarLecturaAsync(int idGuia, long idLecturaGuia)
        {
            throw new NotImplementedException();
        }

        public Task FinalizarLecturaAsync(int idGuia)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<MovimientoStockDto>> ListarAsync(int? idGuia, int? idAlmacen, int? idOperacion)
        {
            throw new NotImplementedException();
        }



        public async Task<IReadOnlyList<GuiaOperadorDto>> ListarGuiasOperadorAsync(int? idEstadoGuia)
        {
            await _validacionesGuia.ExigirOperadorAsync();

            var guia = await _repository.ListarGuiasParaOperadorAsync(_currentUser.IdUsuario, idEstadoGuia);
            return guia.Select(GuiaMapper.ToGuiaOperadorDto).ToList();
            
        }

        public async Task<GuiaOperadorDto> ObtenerMiGuiaAsync(int idGuia)
        {
            await _validacionesGuia.ExigirOperadorAsync();
            var guia = await _repository.ObtenerGuiaOperadorAsync(idGuia, _currentUser.IdUsuario)
                 ?? throw new NegocioException("La guia no existe en tus guias en proceso o en tus guias terminadas. . .");

            ValidacionesGuia.ExigirOperacionesOperador(guia.IdTipoOperacionGuia);
            return GuiaMapper.ToGuiaOperadorDto(guia);
        }

        public async Task<LecturaGuiaDto> RegistrarLecturaAsync(int idGuia, RegistrarLecturaGuiaRequest request)
        {
            await _validacionesGuia.ExigirOperadorAsync();
            var lectura = await RegistrarLecturaEnTransaccionAsync(idGuia, request);
            return GuiaMapper.ToLecturaDto(lectura);
        }

        public async Task TomarGuiaAsync(int idGuia)
        {
            await _validacionesGuia.ExigirOperadorAsync();
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
    }
}


