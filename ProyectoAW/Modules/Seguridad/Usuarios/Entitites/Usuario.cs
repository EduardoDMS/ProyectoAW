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
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        public DateTime? FechaActualizada { get; set; }
        public bool Activo { get; set; } = true;

        public int IdRol { get; set; }
        public Rol? rol { get; set; }

        public ICollection<MovimientoStock>? MovimientoStock { get; set; }
    }
}
