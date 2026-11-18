using Microsoft.EntityFrameworkCore;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Seguridad.Usuarios.Entitites;

namespace ProyectoAW.Modules.Seguridad.Security_Session.Repositories
{
    public class AuthRepository : IAuthRepository
    {

        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ObtenerPorUsernameAsync(string username)
        {
            return await _context.Usuarios.AsNoTracking()
                                .Include(usuario => usuario.rol)
                                .FirstOrDefaultAsync(usuario => usuario.Username == username);
        }
    }
}
