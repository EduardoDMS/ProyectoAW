using ProyectoAW.Modules.Stocks.Services;
using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Maestros.Estados.Entities;
using ProyectoAW.Modules.Maestros.Operaciones.Entities;
using ProyectoAW.Modules.Stocks.Dtos.Request;
using ProyectoAW.Modules.Stocks.Entities;
using ProyectoAW.Modules.Stocks.Repositories;
using ProyectoAW.Modules.Stocks.Factories;
using ProyectoAW.Modules.Seguridad.Security_Session.Permisos.HttpAuth;

namespace ProyectoAW.Modules.Stocks.Validacion
{
    public class ValidacionesGuia : ValidacionesBase
    {


        public ValidacionesGuia(ILecturaGuiaRepository repository, ICurrentUserService currentUser, ITransactionManager transactions, IEnumerable<IGuiaStockProcessor> stockProcessors) : base(repository, currentUser, transactions, stockProcessors)
        {
        }

        public async Task<Guia> ObtenerGuiaAsync(int idGuia)
        {
            return await _repository.ObtenerGuiaParaLecturaAsync(idGuia) ?? throw new NegocioException($"La guia {idGuia} no existe.");
        }

        public async Task ExigirOperadorAsync()
        {
            if (!await _repository.ExisteOperadorAsync(_currentUser.IdUsuario))
                throw new NegocioException("Solo un usuario con el rol Operador puede realizar lecturas de guias;");
        }

        public static void ValidarGuiaPuedeSerTomada(Guia guia)
        {
            ExigirOperacionesOperador(guia.IdTipoOperacionGuia);
            if (guia.IdEstadoGuia != EstadoGuiasIds.Pendiente)
                throw new NegocioException("La guia no esta pendiente ya fue tomada por alguien mas, Finalizando proceso.");
        }


        public static void ValidarGuiaEnProceso(Guia guia, int idOperador)
        {
            if (guia.IdTipoOperacionGuia is not (TipoOperacionGuiaIds.Entrada or TipoOperacionGuiaIds.Salida))
                throw new NegocioException("El operador solo puede atender guias de Entrada o Salida.");

            // CORRECCIÓN: Validar que esté en proceso y asignada al operador
            if (guia.IdEstadoGuia != EstadoGuiasIds.EnProceso)
                throw new NegocioException("La guia no está en proceso.");

            if (guia.IdUsuarioModificacion != idOperador)
                throw new NegocioException("Esta guia está siendo atendida por otro operador.");
        }


        public static void ExigirOperacionesOperador(int idTipoOperacionGuia)
        {
            if (idTipoOperacionGuia is not (TipoOperacionGuiaIds.Entrada or TipoOperacionGuiaIds.Salida))
                throw new NegocioException("El operadorr solo puede atender guia de entrada y salida , otros movimientos externos no se le son permitidos");
        }


        public async Task TomarGuiaTransaccionAsync(int idGuia)
        {
            var guia = await ObtenerGuiaAsync(idGuia);
            ValidarGuiaPuedeSerTomada(guia);

            guia.IdEstadoGuia = EstadoGuiasIds.EnProceso;
            guia.IdUsuarioModificacion = _currentUser.IdUsuario;
                guia.FechaActualizada = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);
            await _repository.GuardarCambiosAsync();

        }


















        private static void ValidarDetallesDeGuia(Guia guia)
        {
            var noTieneDetalles = guia.Detalles.Count == 0;
            var tieneProductosDuplicados = guia.Detalles
                .GroupBy(detalle => detalle.IdProducto)
                .Any(grupo => grupo.Count() > 1);

            if (noTieneDetalles || tieneProductosDuplicados)
                throw new NegocioException("La guía no tiene productos válidos para procesar.");
        }


        private IGuiaStockProcessor ObtenerProcesador(int idTipoOperacionGuia) =>
            _stockProcessors.TryGetValue(idTipoOperacionGuia, out var processor)
                ? processor
                : throw new NegocioException("La guía tiene un tipo de operación inválido.");


        private static void ValidarCantidadesEnteras(Guia guia)
        {
            var contieneCantidadNoEntera = guia.Detalles.Any(detalle =>
                detalle.Cantidad != decimal.Truncate(detalle.Cantidad) ||
                detalle.Cantidad > int.MaxValue);

            if (contieneCantidadNoEntera)
                throw new NegocioException("Esta guía contiene cantidades fraccionarias, pero el stock actual registra unidades enteras. No se aplicó ningún movimiento.");
        }

        private static void ValidarAsignacionEnProceso(Guia guia, int idOperador)
        {
            if (guia.IdEstadoGuia != EstadoGuiasIds.EnProceso)
                throw new NegocioException("La guía no está en proceso.");
            if (guia.IdUsuarioModificacion != idOperador)
                throw new NegocioException("La guía está asignada a otro operador.");
        }

        public IGuiaStockProcessor ValidarGuiaParaFinalizar(Guia guia)
        {
            ExigirOperacionesOperador(guia.IdTipoOperacionGuia);
            ValidarDetallesDeGuia(guia);

            var processor = ObtenerProcesador(guia.IdTipoOperacionGuia);
            processor.ValidarGuia(guia);
            ValidarCantidadesEnteras(guia);
            ValidarAsignacionEnProceso(guia, _currentUser.IdUsuario);
            return processor;
        }



    }
}
