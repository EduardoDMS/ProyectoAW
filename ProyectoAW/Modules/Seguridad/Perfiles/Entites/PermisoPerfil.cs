namespace ProyectoAW.Modules.Seguridad.Perfiles.Entites;

public class PermisoPerfil
{
    public int IdPermisoPerfil { get; set; }
    public int IdPerfil { get; set; }
    public string Modulo { get; set; } = string.Empty;

    public bool Ver { get; set; }
    public bool Crear { get; set; }
    public bool Actualizar { get; set; }
    public bool Eliminar { get; set; }
    public bool Todos { get; set; }

    public Perfil Perfil { get; set; } = null!;
}
