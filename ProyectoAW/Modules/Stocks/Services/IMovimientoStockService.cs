using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Stocks.DTOs;

namespace ProyectoAW.Modules.Stocks.Services
{
    public interface IMovimientoStockService
    {
        Task<IReadOnlyList<Guia>> ListarGuiasParaLecturaAsync(int? idEstadoGuia);
        Task<Guia> ObtenerGuiaParaLecturaAsync(int idGuia);
        Task RegistrarLecturaAsync(int idGuia, RegistrarLecturaGuiaDto request);
    }
}
