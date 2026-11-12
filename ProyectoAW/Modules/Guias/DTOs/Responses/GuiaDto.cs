namespace ProyectoAW.Modules.Guias.DTOs.Responses;

public class GuiaDto
{
    public int IdGuia { get; set; }
    public string NumeroGuia { get; set; } = string.Empty;
    public string TipoOperacion { get; set; } = string.Empty;
    public int? IdMotivoGuia { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public int? IdProveedor { get; set; }
    public string? Proveedor { get; set; }
    public int? IdCliente { get; set; }
    public string? Cliente { get; set; }
    public int? IdAlmacenOrigen { get; set; }
    public string? AlmacenOrigen { get; set; }
    public int? IdAlmacenDestino { get; set; }
    public string? AlmacenDestino { get; set; }
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
