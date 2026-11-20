namespace ProyectoAW.Modules.Maestros.Productos.DTOs.Requests;

public class CrearProductoDto
{
    public string Codigo { get; set; } = string.Empty;
    public string? Nombre { get; set; }
    public string? Lote { get; set; }
    public string Serie { get; set; } = string.Empty;
    public bool Activo { get; set; }

    public string FchRegistro { get; set; } = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);

    public int IdCategoria { get; set; }
    public int IdUnidadMedida { get; set; }

}
