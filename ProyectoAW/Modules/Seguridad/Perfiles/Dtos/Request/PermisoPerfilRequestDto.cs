using System.ComponentModel.DataAnnotations;

namespace ProyectoAW.Modules.Seguridad.Perfiles.Dtos.Request;

public class PermisoPerfilRequestDto
{
    [Required(ErrorMessage = "Indica el módulo del permiso.")]
    [StringLength(60)]
    public string Modulo { get; set; } = string.Empty;

    public bool Ver { get; set; }
    public bool Crear { get; set; }
    public bool Actualizar { get; set; }
    public bool Eliminar { get; set; }
}
