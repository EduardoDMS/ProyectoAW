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

    public DateTime FechaProgramada { get; set; }

    [StringLength(500)]
    public string? Observacion { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Agrega al menos un producto a la guía.")]
    public List<CrearDetalleGuiaDto> Detalles { get; set; } = new();
}
