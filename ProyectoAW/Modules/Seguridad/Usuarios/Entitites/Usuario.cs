using ProyectoAW.Modules.Seguridad.Perfiles.Entites;
using ProyectoAW.Modules.Seguridad.Roles.Entities;
using ProyectoAW.Modules.Stocks.Entities;
//using ProyectoAW.Modules.Stocks.Entities;

namespace ProyectoAW.Modules.Seguridad.Usuarios.Entitites
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FechaRegistro { get; set; } = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);
        public string? FechaActualizada { get; set; }
        public bool Activo { get; set; } = true;

        public int IdRol { get; set; }
        public Rol? rol { get; set; }

        public int IdPerfilUsuario { get; set; }
        public Perfil? PerfilUsuario { get; set; }

        public ICollection<MovimientoStock>? MovimientoStock { get; set; }
    }
}
