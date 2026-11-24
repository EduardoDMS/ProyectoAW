using Microsoft.EntityFrameworkCore;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Seguridad.Usuarios.Entitites;

namespace ProyectoAW.Modules.Seguridad.Usuarios.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(Usuario usuario)
        {
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task CrearAsync(Usuario usuario)
        {
            await _context.Set<Usuario>().AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Usuario usuario)
        {
            _context.Remove(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _context.Set<Usuario>().AsNoTracking()
                .Include(u => u.rol)
                .Include(u => u.PerfilUsuario)
                .ToListAsync();
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Set<Usuario>().AsNoTracking()
                .Include(u => u.rol)
                .Include(u => u.PerfilUsuario)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);
        }

        public async Task<Usuario?> ObtenerPorUsernameAsync(string username)
        {
            return await _context.Set<Usuario>().AsNoTracking()
                .Include(u => u.rol)
                .Include(u => u.PerfilUsuario)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> TieneRegistrosAsociadosAsync(int idUsuario)
        {
            return await _context.LecturasGuia.AnyAsync(lectura => lectura.IdUsuarioOperador == idUsuario)
                || await _context.MovimientoStock.AnyAsync(movimiento => movimiento.IdUsuarioOperador == idUsuario)
                || await _context.Guias.AnyAsync(guia =>
                    guia.IdUsuarioRegistro == idUsuario || guia.IdUsuarioModificacion == idUsuario);
        }
    }
}
