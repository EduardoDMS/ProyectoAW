using Microsoft.EntityFrameworkCore;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Guias.Entities;

namespace ProyectoAW.Modules.Guias.Repositories;

public class GuiaRepository : IGuiaRepository
{
    private readonly AppDbContext _context;

    public GuiaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Guia>> ObtenerTodosAsync()
    {
        return await ConsultaGuias()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Guia?> ObtenerPorIdAsync(int id)
    {
        return await ConsultaGuias()
            .FirstOrDefaultAsync(guia => guia.IdGuia == id);
    }

    public async Task<string?> ValidarReferenciasAsync(Guia guia)
    {
        if (!await _context.Usuarios.AnyAsync(usuario => usuario.IdUsuario == guia.IdUsuarioRegistro))
            return "No existe el usuario que registra la guía. Vuelve a iniciar sesión.";

        if (guia.IdProveedor.HasValue &&
            !await _context.Proveedores.AnyAsync(proveedor => proveedor.IdProveedor == guia.IdProveedor.Value))
            return $"No existe el proveedor con ID {guia.IdProveedor.Value}.";

        if (guia.IdCliente.HasValue &&
            !await _context.Clientes.AnyAsync(cliente => cliente.IdCliente == guia.IdCliente.Value))
            return $"No existe el cliente con ID {guia.IdCliente.Value}.";

        if (guia.IdAlmacenOrigen.HasValue &&
            !await _context.Almacenes.AnyAsync(almacen => almacen.IdAlmacen == guia.IdAlmacenOrigen.Value))
            return $"No existe el almacén de origen con ID {guia.IdAlmacenOrigen.Value}.";

        if (guia.IdAlmacenDestino.HasValue &&
            !await _context.Almacenes.AnyAsync(almacen => almacen.IdAlmacen == guia.IdAlmacenDestino.Value))
            return $"No existe el almacén de destino con ID {guia.IdAlmacenDestino.Value}.";

        foreach (var idProducto in guia.Detalles.Select(detalle => detalle.IdProducto).Distinct())
        {
            if (!await _context.Productos.AnyAsync(producto => producto.IdProducto == idProducto))
                return $"No existe el producto con ID {idProducto}.";
        }

        return null;
    }

    public async Task CrearAsync(Guia guia)
    {
        await _context.Guias.AddAsync(guia);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Guia guia)
    {
        // ObtenerPorIdAsync devuelve la guía seguida por EF Core; basta con guardar sus cambios.
        await _context.SaveChangesAsync();
    }

    private IQueryable<Guia> ConsultaGuias()
    {
        return _context.Guias
            .Include(guia => guia.TipoOperacion)
            .Include(guia => guia.Estado)
            .Include(guia => guia.Motivo)
            .Include(guia => guia.Proveedor)
            .Include(guia => guia.Cliente)
            .Include(guia => guia.AlmacenOrigen)
            .Include(guia => guia.AlmacenDestino)
            .Include(guia => guia.Detalles)
                .ThenInclude(detalle => detalle.Producto);
    }

    public async Task<List<Guia>> ListarGuiasTipoOperacionAsync(int idTipoOperacion)
    {
        return await ConsultaGuias()
                    .Where(g => g.IdTipoOperacionGuia == idTipoOperacion)
                    .AsNoTracking()
                    .ToListAsync();
                
    }
}
