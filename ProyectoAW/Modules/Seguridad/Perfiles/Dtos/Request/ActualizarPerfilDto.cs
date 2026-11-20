using System.ComponentModel.DataAnnotations;

namespace ProyectoAW.Modules.Seguridad.Perfiles.Dtos.Request;

public class ActualizarPerfilDto
{
    [Required(ErrorMessage = "Ingresa el nombre del perfil.")]
    [StringLength(100)]
    public string NombrePerfil { get; set; } = string.Empty;

    public List<PermisoPerfilRequestDto> Permisos { get; set; } = new();
}
