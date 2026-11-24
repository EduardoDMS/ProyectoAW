using System.ComponentModel.DataAnnotations;

namespace ProyectoAW.Modules.Guias.DTOs.Requests;

public class CrearGuiaDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un motivo.")]
    public int IdMotivoGuia { get; set; }

    public int? IdProveedor { get; set; }
    public int? IdCliente { get; set; }
    public int? IdAlmacenOrigen { get; set; }
    public int? IdAlmacenDestino { get; set; }

    public int? IdTipoOperacion { get; set; }

    [Required]
    public string FechaProgramada { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Observacion { get; set; }

    [StringLength(100)]
    public string TipoDocumentoReferencia { get; set; } = string.Empty;

    [StringLength(100)]
    public string NumeroDocumentoReferencia { get; set; } = string.Empty;

    [Required]
    [MinLength(1, ErrorMessage = "Agrega al menos un producto a la guía.")]
    public List<CrearDetalleGuiaDto> Detalles { get; set; } = new();
}
