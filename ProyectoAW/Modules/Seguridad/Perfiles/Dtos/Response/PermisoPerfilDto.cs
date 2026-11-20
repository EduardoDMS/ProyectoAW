namespace ProyectoAW.Modules.Seguridad.Perfiles.Dtos.Response;

public class PermisoPerfilDto
{
    public string Modulo { get; set; } = string.Empty;
    public bool Ver { get; set; }
    public bool Crear { get; set; }
    public bool Actualizar { get; set; }
    public bool Eliminar { get; set; }
}
