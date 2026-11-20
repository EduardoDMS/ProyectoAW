using ProyectoAW.Modules.Seguridad.Usuarios.Entitites;

namespace ProyectoAW.Modules.Seguridad.Perfiles.Entites
{
    public class Perfil
    {
        public int IdPerfil { get; set; }
        public string NombrePerfil { get; set; } = string.Empty;
        public ICollection<PermisoPerfil> Permisos { get; set; } = new List<PermisoPerfil>();

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
