using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Stocks.Dtos.Request;
using ProyectoAW.Modules.Stocks.Dtos.Response;

namespace ProyectoAW.Modules.Stocks.Services
{
    public interface IMovimientoStockService
    {
        Task TomarGuiaAsync(int idGuia);
        Task<IReadOnlyList<GuiaOperadorDto>> ListarGuiasOperadorAsync(int? idEstadoGuia);
        Task<GuiaOperadorDto> ObtenerMiGuiaAsync(int idGuia);
        Task<LecturaGuiaDto> RegistrarLecturaAsync(int idGuia, RegistrarLecturaGuiaRequest request);
        Task<bool> EliminarLecturaAsync(int idGuia, long idLecturaGuia);
        Task FinalizarLecturaAsync(int idGuia);
        Task<IReadOnlyList<MovimientoStockDto>> ListarAsync(int? idGuia, int? idAlmacen, int? idOperacion);
    }
}
