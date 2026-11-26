using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Maestros.Operaciones.Entities;
using ProyectoAW.Modules.Stocks.Dtos.Request;
using ProyectoAW.Modules.Stocks.Repositories;
using ProyectoAW.Modules.Stocks.Entities;
using ProyectoAW.Modules.Stocks.Services;
using ProyectoAW.Modules.Seguridad.Security_Session.Permisos.HttpAuth;

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
            return _stockProcessors.TryGetValue(idTipoOperacionGuia, out var processor) 
                ? processor
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

        private static void ValidarUbicacionesIndicadas(
            bool requiereOrigen, bool requiereDestino, bool tieneOrigen, bool tieneDestino, int idProducto)
        {
            if (requiereOrigen != tieneOrigen || requiereDestino != tieneDestino)
                throw new NegocioException($"La lectura del producto {idProducto} no tiene las ubicaciones requeridas.");
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

            if (guia.IdAlmacenDestino is not int idAlmacenDestino)
                throw new NegocioException("La guía no tiene almacén de destino.");
            if (!await UbicacionActivaAsync(idAlmacenDestino, destino!.Value, false))
                throw new NegocioException("La ubicación no está activa o no pertenece al almacén de destino de la guía.");
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

        private static void ValidarUbicacionOrigenLeida(
            ProyectoAW.Modules.Stocks.Entities.LecturaGuia lectura, bool requiereOrigen, IReadOnlySet<int> ubicacionesOrigenValidas)
        {
            if (requiereOrigen && !ubicacionesOrigenValidas.Contains(lectura.IdUbicacionOrigen!.Value))
                throw new NegocioException($"La ubicación de origen del producto {lectura.IdProducto} dejó de estar activa o no pertenece al almacén.");
        }

        private static void ValidarUbicacionDestinoLeida(
            ProyectoAW.Modules.Stocks.Entities.LecturaGuia lectura,
            bool salida,
            bool requiereUbicacionDestino,
            IReadOnlySet<int> ubicacionesDestinoValidas)
            {
                if (!requiereUbicacionDestino)
                    return;

                if (!ubicacionesDestinoValidas.Contains(lectura.IdUbicacionDestino!.Value))
                {
                    var mensaje = salida
                        ? $"La ubicación elegida para el despacho del producto {lectura.IdProducto} no está habilitada para despacho en el almacén de origen."
                        : $"La ubicación destino del producto {lectura.IdProducto} dejó de estar activa o no pertenece al almacén asignado.";
                    throw new NegocioException(mensaje);
                }
            }


        private Task<IReadOnlySet<int>> ObtenerUbicacionesOrigenValidasAsync(
            Guia guia,
            IReadOnlyList<ProyectoAW.Modules.Stocks.Entities.LecturaGuia> lecturas,
            IGuiaStockProcessor processor)
        {
            if (!processor.RequiereUbicacionOrigen)
                return Task.FromResult<IReadOnlySet<int>>(new HashSet<int>());

            var idsOrigen = lecturas
                .Where(lectura => lectura.IdUbicacionOrigen.HasValue)
                .Select(lectura => lectura.IdUbicacionOrigen!.Value)
                .Distinct()
                .ToArray();

            return _repository.ObtenerUbicacionesActivasAsync(
                guia.IdAlmacenOrigen!.Value, idsOrigen, false);
        }


        private Task<IReadOnlySet<int>> ObtenerUbicacionesDestinoValidasAsync(
        int idAlmacenDestino,
        IReadOnlyList<ProyectoAW.Modules.Stocks.Entities.LecturaGuia> lecturas,
        bool requiereUbicacionDestino,
        bool esDespacho)
        {
            if (!requiereUbicacionDestino)
                return Task.FromResult<IReadOnlySet<int>>(new HashSet<int>());

            var idsDestino = lecturas
                .Where(lectura => lectura.IdUbicacionDestino.HasValue)
                .Select(lectura => lectura.IdUbicacionDestino!.Value)
                .Distinct()
                .ToArray();

            return _repository.ObtenerUbicacionesActivasAsync(
                idAlmacenDestino, idsDestino, esDespacho);
        }


        private static void ValidarUbicacionLeida(
        ProyectoAW.Modules.Stocks.Entities.LecturaGuia lectura,
        IGuiaStockProcessor processor,
        bool salida,
        bool requiereUbicacionDespacho,
        IReadOnlySet<int> ubicacionesOrigenValidas,
        IReadOnlySet<int> ubicacionesDestinoValidas)
        {
            var requiereDestino = salida
                ? requiereUbicacionDespacho
                : processor.RequiereUbicacionDestino;
            ValidarUbicacionesIndicadas(
                processor.RequiereUbicacionOrigen, requiereDestino,
                lectura.IdUbicacionOrigen.HasValue, lectura.IdUbicacionDestino.HasValue,
                lectura.IdProducto);

            ValidarUbicacionOrigenLeida(lectura, processor.RequiereUbicacionOrigen, ubicacionesOrigenValidas);
            ValidarUbicacionDestinoLeida(
                lectura, salida, requiereDestino, ubicacionesDestinoValidas);
        }


        private static void ValidarLecturasContraUbicaciones(
        IReadOnlyList<ProyectoAW.Modules.Stocks.Entities.LecturaGuia> lecturas,
        IGuiaStockProcessor processor,
        bool salida,
        bool requiereUbicacionDespacho,
        IReadOnlySet<int> ubicacionesOrigenValidas,
        IReadOnlySet<int> ubicacionesDestinoValidas)
        {
            foreach (var lectura in lecturas)
                ValidarUbicacionLeida(lectura, processor, salida, requiereUbicacionDespacho,
                    ubicacionesOrigenValidas, ubicacionesDestinoValidas);
        }


        public async Task ValidarUbicacionesFinalesAsync(
        Guia guia,
        IReadOnlyList<ProyectoAW.Modules.Stocks.Entities.LecturaGuia> lecturas,
        IGuiaStockProcessor processor,
        bool requiereUbicacionDespacho)
            {
                // Consulta las ubicaciones en bloque y reutiliza esos resultados al validar cada lectura.
                var salida = guia.IdTipoOperacionGuia == TipoOperacionGuiaIds.Salida;
                var ubicacionesOrigenValidas = await ObtenerUbicacionesOrigenValidasAsync(
                    guia, lecturas, processor);

                var requiereUbicacionDestino = salida ? requiereUbicacionDespacho : processor.RequiereUbicacionDestino;
                var almacenDestino = salida ? guia.IdAlmacenOrigen!.Value : guia.IdAlmacenDestino!.Value;
                var ubicacionesDestinoValidas = await ObtenerUbicacionesDestinoValidasAsync(
                    almacenDestino, lecturas, requiereUbicacionDestino, salida);

                ValidarLecturasContraUbicaciones(
                    lecturas, processor, salida, requiereUbicacionDespacho,
                    ubicacionesOrigenValidas, ubicacionesDestinoValidas);
            }   

    }
}
