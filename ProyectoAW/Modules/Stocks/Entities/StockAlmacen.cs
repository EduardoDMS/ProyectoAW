using ProyectoAW.Modules.Maestros.Almacenes.Entities;
using ProyectoAW.Modules.Maestros.Productos.Entities;

namespace ProyectoAW.Modules.Stocks.Entities
{
    public class StockAlmacen
    {
        public int IdStockAlmacen { get; set; }
        public int IdAlmacen { get; set; }
        public int IdProducto { get; set; }
        public int CantidadActual { get; set; }
        public string FechaUltimoRegitro { get; set; } = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);

        public Almacen? Almacen { get; set; }
        public Producto? Producto { get; set; }
    }
}
