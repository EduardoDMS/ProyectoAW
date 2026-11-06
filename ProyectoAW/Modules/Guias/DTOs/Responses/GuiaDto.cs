namespace ProyectoAW.Modules.Guias.DTOs.Responses;

public class GuiaDto
{
    public int IdGuia { get; set; }
    public string NumeroGuia { get; set; } = string.Empty;
    public string TipoOperacion { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public int IdProveedor { get; set; }
    public string Proveedor { get; set; } = string.Empty;
    public DateTime FechaProgramada { get; set; }
    public string? Observacion { get; set; }
    public List<DetalleGuiaDto> Detalles { get; set; } = new();
}

public class DetalleGuiaDto
{
    public int IdProducto { get; set; }
    public string Producto { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
}
