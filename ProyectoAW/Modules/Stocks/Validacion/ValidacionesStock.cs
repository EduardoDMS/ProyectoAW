using ProjectAW.Modules.Stocks.Services;
using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Migrations;
using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Maestros.Estados.Entities;
using ProyectoAW.Modules.Maestros.Operaciones.Entities;
using ProyectoAW.Modules.Seguridad.Security.HttpAuth;
using ProyectoAW.Modules.Stocks.Dtos.Request;
using ProyectoAW.Modules.Stocks.Repositories;

namespace ProyectoAW.Modules.Stocks.Validacion
{
    public class ValidacionesStock : ValidacionesBase
    {
        public ValidacionesStock(ILecturaGuiaRepository repository, ICurrentUserService currentUser, ITransactionManager transactions, IEnumerable<IGuiaStockProcessor> stockProcessors) : base(repository, currentUser, transactions, stockProcessors)
        {
        }

        public async Task ValidarProductoLeidoAsync(Guia guia, RegistrarLecturaGuiaRequest request)
        {
            var detalle = guia.Detalles.FirstOrDefault(d => d.IdProducto == request.IdProducto)
                        ?? throw new NegocioException("El producto leido nopertenece a esta guia");

            var codigoProducto = await _repository.ObtenerCodigoProductoAsync(detalle.IdProducto)
                        ?? throw new NegocioException("El producto de la guia ya no existe");

            if (!string.Equals(codigoProducto.Trim(), request.CodigoLeido.Trim(), StringComparison.OrdinalIgnoreCase))
                throw new NegocioException("El codigo leido no corresponde al producto seleccionado");
        }

        //public static string? Normalizar(string? valor) => string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();

    }
}
