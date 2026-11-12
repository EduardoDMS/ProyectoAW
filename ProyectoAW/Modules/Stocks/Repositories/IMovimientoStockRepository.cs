using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Stocks.Entities;

namespace ProyectoAW.Modules.Stocks.Repositories
{
    public interface IMovimientoStockRepository
    {
        Task<Guia?> ObtenerGuiaParaLecturaAsync(int idGuia);
        Task<IReadOnlyList<Guia>> ListarGuiasParaLecturaAsync(int? idEstadoGuia); //listara las lecturas que aun no an sido iniciadas mientras que el otro lista en general
        Task<IReadOnlyList<Guia>> ListarGuiasParaOperadorAsync(int idOperador, int? estadoGuia);
        Task<IReadOnlyList<Guia>> ListarLecturasAsync(int idGuia, int idOperador);
        Task<IReadOnlyList<int>> ObtenerUbicacionesActivasAsync(int idAlmacen,
                                                                IReadOnlyList<int> idsUbicaciones,
                                                                bool soloActivos);
        Task<List<MovimientoStock>> ListarMovimientosStockAsync();

        Task<LecturaGuia?> ObtenerLecturaGuia(int idGuia, long idLecturaGuia,int idOperador);
        Task RegistrarLecturaAsync(LecturaGuia lectura, Guia guia);

        Task GuardarCambiosAsync();
        
        Task<bool> ExisteOperadorAsync(int idOperador);
        Task<string?> ObtenerCodigoProductoAsync(int idProducto);
        Task<long> ObtenerCantidadStockAsync(int idUbicacion, int idProducto);
        Task<int> ObtenerCantidadLeidaAsync(int idGuia, int idProducto);
        Task<bool> UbicacionPerteneceAlmacenAsync(int idUbicacion, int idAlmacen);

        void AgregarMovimientoAsync(MovimientoStock movimiento);
        void AgregarLectura(LecturaGuia lectura);
        void EliminarLectura(LecturaGuia lectura);


    }
}

