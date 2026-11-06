using System.ComponentModel.DataAnnotations;

namespace ProyectoAW.Modules.Maestros.Productos.DTOs.Responses;

public class ProductoDto
{
    public int IdProducto { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Lote { get; set; }
    public string Serie { get; set; } = string.Empty;
    public bool Activo { get; set; } 
    public int IdCategoria { get; set; }
    public int IdUnidadMedida { get; set; }
}
