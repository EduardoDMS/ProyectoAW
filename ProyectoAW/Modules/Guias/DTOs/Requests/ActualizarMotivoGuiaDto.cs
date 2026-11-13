using System.ComponentModel.DataAnnotations;

namespace ProyectoAW.Modules.Guias.DTOs.Requests;

public class ActualizarMotivoGuiaDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una operacion.")]
    public int IdTipoOperacionGuia { get; set; }

    [Required(ErrorMessage = "Ingresa el nombre del motivo.")]
    [StringLength(100)]
    public string NombreMotivo { get; set; } = string.Empty;

    public bool RequiereProveedor { get; set; }
    public bool RequiereCliente { get; set; }
    public bool RequiereAlmacenOrigen { get; set; }
    public bool RequiereAlmacenDestino { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un tipo de comportamiento.")]
    public int IdTipoComportamiento { get; set; }

    public bool Activo { get; set; }
}
