// using ProyectoAW.Modules.Seguridad.Usuarios.Entitites;

namespace ProyectoAW.Modules.Seguridad.Roles.Entities
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; } = string.Empty;
        // public string Abreviatura { get; set; } = string.Empty;

        //public ICollection<Usuario>? Usuario { get; set; }
    }
}
