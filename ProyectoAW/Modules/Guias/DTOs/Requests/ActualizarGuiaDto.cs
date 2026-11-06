using System.ComponentModel.DataAnnotations;

namespace ProyectoAW.Modules.Guias.DTOs.Requests;

public class ActualizarGuiaDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un proveedor.")]
    public int IdProveedor { get; set; }

    public DateTime FechaProgramada { get; set; }

    [StringLength(500)]
    public string? Observacion { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Agrega al menos un producto a la guía.")]
    public List<CrearDetalleGuiaDto> Detalles { get; set; } = new();
}
