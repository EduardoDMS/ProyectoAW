using ProyectoAW.Modules.Maestros.Productos.Entities;
using ProyectoAW.Modules.Maestros.Ubicaciones.Entities;

namespace ProyectoAW.Modules.Stocks.Entities
{
    public class StockUbicacion
    {
        public int IdStockUbicacion { get; set; }
        public int IdProducto { get; set; }
        public int IdUbicacion { get; set; }
        public int CantidadActual { get; set; }
        public string FechaUltimoRegistro { get; set; } = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);
        public Producto? Producto { get; set; }
        public Ubicacion? Ubicacion { get; set; }
    }
}
