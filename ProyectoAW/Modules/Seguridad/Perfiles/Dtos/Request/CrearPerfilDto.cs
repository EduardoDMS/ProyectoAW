using System.ComponentModel.DataAnnotations;

namespace ProyectoAW.Modules.Seguridad.Perfiles.Dtos.Request;

public class CrearPerfilDto
{
    [Required(ErrorMessage = "Ingresa el nombre del perfil.")]
    [StringLength(100)]
    public string NombrePerfil { get; set; } = string.Empty;
    public int IdRol { get; set; }

    public List<PermisoPerfilRequestDto> Permisos { get; set; } = new();
}
