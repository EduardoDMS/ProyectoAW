using ProyectoAW.Modules.Maestros.Productos.Entities;

namespace ProyectoAW.Modules.Guias.Entities;

public class DetalleGuia
{
    public int IdDetalleGuia { get; set; }

    public int IdGuia { get; set; }
    public Guia Guia { get; set; } = null!;

    public int IdProducto { get; set; }
    public Producto Producto { get; set; } = null!;

    public decimal Cantidad { get; set; }
}
