using ProjectAW.Modules.Stocks.Services;
using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Migrations;
using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Maestros.Operaciones.Entities;
using ProyectoAW.Modules.Seguridad.Security.HttpAuth;
using ProyectoAW.Modules.Stocks.Dtos.Request;
using ProyectoAW.Modules.Stocks.Repositories;

namespace ProyectoAW.Modules.Stocks.Validacion
{
    public class ValidacionesUbicacion : ValidacionesBase
    {
        public ValidacionesUbicacion(ILecturaGuiaRepository repository, ICurrentUserService currentUser, ITransactionManager transactions, IEnumerable<IGuiaStockProcessor> stockProcessors) : base(repository, currentUser, transactions, stockProcessors)
        { }

        public async Task ValidarUbicacionesAsync(Guia guia, int? origen, int? destino)
        {
            var processor = ObtenerProcesador(guia.IdTipoOperacionGuia);
            var salida = guia.IdTipoOperacionGuia == TipoOperacionGuiaIds.Salida;
            var requiereUbicacionDespacho = await RequiereUbicacionesDespachoAsync(guia, salida);
            var requiereDestino = salida ? requiereUbicacionDespacho : processor.RequiereUbicacionDestino;

            ValidarUbicacionesIndicadas(processor.RequiereUbicacionOrigen, requiereDestino, origen, destino);
            await ValidarUbicacionOrigenAsync(guia, processor, origen);
            await ValidarUbicacionesDespachoAsync(guia, salida, requiereUbicacionDespacho, destino);
            await ValidarUbicacionDestinoAsync(guia, processor, salida, destino);
        }

        public IGuiaStockProcessor ObtenerProcesador(int idTipoOperacionGuia)
        {
            return _stockProcessors.TryGetValue(idTipoOperacionGuia, out var processor) ? processor
                : throw new NegocioException("La guia tiene un tipo de operacioninvalido");
        }

        public Task<bool> RequiereUbicacionesDespachoAsync(Guia guia, bool salida)
        {
            if (!salida || guia.IdAlmacenOrigen is not int idAlmacenOrigen)
                return Task.FromResult(false);

            return _repository.TieneUbicacionesDeDespacho(idAlmacenOrigen);
        }

        public static void ValidarUbicacionesIndicadas(bool requiereOrigen, bool requiereDestino, int? origen, int? destino)
        {
            var origenIndicado = origen is > 0;
            var destinoIndicado = destino is > 0;
            if (requiereOrigen != origenIndicado || requiereDestino != destinoIndicado)
                throw new NegocioException("Indica las ubicaciones correspondientes al tipo de operacion de esta guia.");
        }

        public async Task ValidarUbicacionOrigenAsync(Guia guia, IGuiaStockProcessor processor, int? origen)
        {
            if (!processor.RequiereUbicacionOrigen)
                return;

            if (guia.IdAlmacenOrigen is not int idAlmacenOrigen || !await UbicacionActivaAsync(idAlmacenOrigen, origen!.Value, false))
                throw new NegocioException("La ubicacion de origen no esta activa o no pertenece al almacen de origen");
        }

        public async Task ValidarUbicacionesDespachoAsync(Guia guia, bool salida, bool requiereUbicaionDespacho, int? destino)
        {
            if (!salida || !requiereUbicaionDespacho)
                return;

            if (!await UbicacionActivaAsync(
                guia.IdAlmacenOrigen!.Value, destino!.Value, true))
                throw new NegocioException("La ubicacion elegida no esta habilitada para despacho en el almacen de origen");
        }


        public async Task ValidarUbicacionDestinoAsync(Guia guia, IGuiaStockProcessor processor, bool salida, int? destino)
        {
            if (salida || !processor.RequiereUbicacionDestino)
                return;

            if (guia.IdAlmacenDestino is not int idAlmacenDestino || !await UbicacionActivaAsync(idAlmacenDestino, destino!.Value, false))
                throw new NegocioException("La ubicacion de destino no esta activa o no pertenece al almacen de destino. ");
        }

        public async Task<bool> UbicacionActivaAsync(int idAlmacen, int idUbicacion, bool soloDespacho)
        {
            var ubicaciones = await _repository.ObtenerUbicacionesActivasAsync(
                idAlmacen, [idUbicacion], soloDespacho);
            return ubicaciones.Contains(idUbicacion);
        }

        public async Task ValidarUbicacionesYDisponibilidadAsync(Guia guia, int idGuia, RegistrarLecturaGuiaRequest request)
        {
            await ValidarUbicacionesAsync(guia, request.IdUbicacionOrigen, request.IdUbicacionDestino);
            if (guia.IdTipoOperacionGuia != TipoOperacionGuiaIds.Salida)
                return;

            await ValidarDisponibilidadSalidaAsync(idGuia, request);
        }

        public async Task ValidarDisponibilidadSalidaAsync(int idGuia, RegistrarLecturaGuiaRequest request)
        {
            var idUbicacionOrigen = request.IdUbicacionOrigen!.Value;
            var stockActual = await _repository.ObtenerCantidadStockAsync(idUbicacionOrigen, request.IdProducto);
            var yaRegistrado = await _repository.ObtenerCantidadLeidaAsync(
                idGuia, request.IdProducto, idUbicacionOrigen);

            if (yaRegistrado + request.Cantidad > stockActual)
                throw new NegocioException($"La Ubicacion de origen {Math.Max(stockActual - yaRegistrado, 0)}solo tiene unidades disponibles de este producto");
        }






    }
}
