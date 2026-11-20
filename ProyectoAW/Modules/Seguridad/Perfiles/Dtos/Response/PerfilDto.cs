namespace ProyectoAW.Modules.Seguridad.Perfiles.Dtos.Response;

public class PerfilDto
{
    public int IdPerfil { get; set; }
    public int IdRol { get; set; }
    public string NombrePerfil { get; set; } = string.Empty;
    public List<PermisoPerfilDto> Permisos { get; set; } = new();
}
