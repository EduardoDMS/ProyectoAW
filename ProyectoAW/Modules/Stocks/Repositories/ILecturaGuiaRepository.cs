using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Stocks.Entities;

namespace ProyectoAW.Modules.Stocks.Repositories
{
    public interface ILecturaGuiaRepository 
    {
        Task<Guia?> ObtenerGuiaParaLecturaAsync(int idGuia);
        Task<Guia?> ObtenerGuiaOperadorAsync(int idGuia, int idOperador);

        Task<IReadOnlyList<Guia>> ListarGuiasParaLecturaAsync(int? idEstadoGuia);
        Task<IReadOnlyList<Guia>> ListarGuiasParaOperadorAsync(int idOperador, int? intEstadoGuia);
        Task<IReadOnlyList<LecturaGuia>> ListarLecturasAsync(int idGuia, int idOperador);
        Task<LecturaGuia?> ObtenerLecturaGuiaAsync(int idGuia, long idLecturaGuia, int idOperador);


        Task<IReadOnlySet<int>> ObtenerUbicacionesActivasAsync(
            int idAlmacen,
            IReadOnlyCollection<int> idsUbicaciones,
            bool soloDespacho);
        Task<bool> ExisteOperadorAsync(int idOperador);
        Task<bool> TieneUbicacionesDeDespacho(int idAlmacen);
        Task<string?> ObtenerCodigoProductoAsync(int idProducto);
        Task<long> ObtenerCantidadStockAsync(int idUbicacion, int idProducto);
        Task<long> ObtenerCantidadLeidaAsync(int idGuia, int idProducto,int idUbicacion);

        Task GuardarCambiosAsync();

        void AgregarLectura(LecturaGuia lectura);
        void EliminarLectura(LecturaGuia lectura);
    }
}