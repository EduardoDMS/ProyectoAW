using ProyectoAW.Modules.Maestros.Categorias.Entities;
//using ProyectoAW.Modules.Stocks.Entities;
using ProyectoAW.Modules.Maestros.UnidadesMedida.Entities;
using ProyectoAW.Modules.Stocks.Entities;
//using ProyectoAW.Modules.Stocks.Entities;

namespace ProyectoAW.Modules.Maestros.Productos.Entities;

public class Producto
{
    public int IdProducto { get; set; }
    public string Codigo { get; set; }=string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Lote { get; set; }
    public string Serie { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    public DateTime FchRegistro { get; set; } = DateTime.Now;
    public DateTime? FchModificacion { get; set; }


    public int IdCategoria { get; set; }
    public Categoria? Categoria { get; set; }
    public int IdUnidadMedida { get; set; }
    public UnidadMedida? UnidadMedida { get; set; }


    public ICollection<MovimientoStock>? MovimientoStocks { get; set; }
    public ICollection<StockAlmacen>? StockAlmacen { get; set; }
   // conexion a nuevas tablas
    public int CodigoUsuarioModificacion { get; set; }

}
