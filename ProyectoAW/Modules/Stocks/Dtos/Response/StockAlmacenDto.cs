using ProyectoAW.Modules.Maestros.Almacenes.Entities;
using ProyectoAW.Modules.Maestros.Productos.Entities;

namespace ProyectoAW.Modules.Stocks.Dtos.Response
{
    public class StockAlmacenDto
    {
        public int IdStockAlmacen { get; set; }
        public int IdAlmacen { get; set; }
        public int IdProducto { get; set; }
        public int CantidadActual { get; set; }

        public Almacen? almacen { get; set; }
        public Producto? producto { get; set; }

    }
}
