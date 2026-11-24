
using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Modules.Maestros.Productos.DTOs.Requests;
using ProyectoAW.Modules.Maestros.Productos.DTOs.Responses;
using ProyectoAW.Modules.Maestros.Productos.Entities;
using ProyectoAW.Modules.Maestros.Productos.Repositories;
using ProyectoAW.Modules.Stocks.Dtos.Response;
using ProyectoAW.Modules.Stocks.Entities;
//using ProyectoAW.Infrastructure.Errors;

namespace ProyectoAW.Modules.Maestros.Productos.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _repository;

    public ProductoService(IProductoRepository repository)
    {
        _repository = repository;
    }

    // preguntar si retorna toda la clase menos la fecha p 
    public async Task<List<ProductoDto>> ObtenerTodosAsync()
    {
        var productos=await _repository.ObtenerTodosAsync();

        return productos.Select(p => new ProductoDto
        {
            IdProducto = p.IdProducto,
            Codigo = p.Codigo,
            Nombre = p.Nombre,
            Lote = p.Lote,
            Serie = p.Serie,
            Activo = p.Activo,
            IdCategoria = p.IdCategoria,
            NombreCategoria = p.Categoria?.Nombre ?? string.Empty,
            IdUnidadMedida = p.IdUnidadMedida,
            NombreUnidadMedida = p.UnidadMedida?.Nombre ?? string.Empty
        }).ToList();
    }

    public async Task<ProductoDto?> ObtenerPorIdAsync(int id)
    {
        var producto = await _repository.ObtenerPorIdAsync(id);

        if (producto == null)
            throw new NegocioException("Producto no encontrado / No existe.");

        return new ProductoDto
        {
            IdProducto = producto.IdProducto,
            Codigo = producto.Codigo,
            Nombre = producto.Nombre,
            Lote = producto.Lote,
            Serie = producto.Serie,
            Activo = producto.Activo,
            IdCategoria = producto.IdCategoria,
            IdUnidadMedida = producto.IdUnidadMedida
        };
    }

    public async Task<ProductoDto?> ObtenerPorCodigoAsync(string codigo)
    {
        var producto = await _repository.ObtenerPorCodigoAsync(codigo);

        if (producto == null)
            throw new NegocioException("No se encontró un producto con ese código.");

        return new ProductoDto
        {
            IdProducto = producto.IdProducto,
            Codigo = producto.Codigo,
            Nombre = producto.Nombre,
            Lote = producto.Lote,
            Serie = producto.Serie,
            Activo = producto.Activo,
            IdCategoria = producto.IdCategoria,
            IdUnidadMedida = producto.IdUnidadMedida
        };
    }

    public async Task<ProductoDto> CrearAsync(CrearProductoDto dto)
    {
        var codigo = dto.Codigo.Trim();

        var productoExistente = await _repository.ObtenerPorCodigoAsync(codigo);

        if (productoExistente != null)
            throw new NegocioException("Ya existe un producto con ese código.");

        var producto = new Producto
        {
            Codigo = codigo,
            Nombre = dto.Nombre?.Trim(),
            Lote = dto.Lote,
            Serie = dto.Serie,
            Activo = dto.Activo,
            FchRegistro = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now),
            IdCategoria = dto.IdCategoria,
            IdUnidadMedida = dto.IdUnidadMedida
        };

        await _repository.CrearAsync(producto);

        return new ProductoDto
        {
            IdProducto = producto.IdProducto,
            Codigo = producto.Codigo,
            Nombre = producto.Nombre,
            Lote = producto.Lote,
            Serie = producto.Serie,
            Activo = producto.Activo,
            IdCategoria = producto.IdCategoria
        };
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarProductoDto dto)
    {
        var producto = await _repository.ObtenerPorIdAsync(id);

        if(producto == null)
            throw new NegocioException("Producto no encontrado.");

        var codigo=dto.Codigo.Trim();

        var productoConMismoCodigo = await _repository.ObtenerPorCodigoAsync(codigo);

        if(productoConMismoCodigo!=null && productoConMismoCodigo.IdProducto!=id)
        {
            throw new NegocioException("Ya existe otro producto con ese código.");
        }

        producto.Codigo = codigo;
        producto.Nombre=dto.Nombre?.Trim();
        producto.Lote = dto.Lote;
        producto.Serie = dto.Serie;
        producto.Activo = dto.Activo;
        producto.FchModificacion = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);

        producto.IdCategoria = dto.IdCategoria;
        producto.IdUnidadMedida = dto.IdUnidadMedida;
        
        await _repository.ActualizarAsync(producto);

        return true;
    }

    public async Task<bool> CambiarEstadoAsync(int id, bool activo)
    {
        var producto = await _repository.ObtenerPorIdAsync(id);

        if (producto == null)
            throw new NegocioException("Producto no encontrado.");

        producto.Activo = activo;

        await _repository.ActualizarAsync(producto);

        return true;
    }

    public async Task EliminarProductoAsync(int id)
    {
        var producto = await _repository.ObtenerPorIdAsync(id);

        if (producto == null)
            throw new NegocioException("No se encontro un producto con ese id para eliminar");

        var productoMovimientosValidos = await _repository.TieneMovimientosAsociadosAsync(id);
        if (productoMovimientosValidos)
            throw new Exception("No se puede borrar el producto se encuentra en un registro de un movimiento actual");

        var productoStockValido = await _repository.TieneStockAsociadoAsync(id);
        if (productoStockValido)
            throw new Exception("No se puede borrar si esta asociado a un stock <0");

        await _repository.EliminarProductoAsync(producto);
    }

    public async Task<List<ProductoAlmacenDto>> ListarPorAlmacenAsync(int idAlmacen)
    {
        if (idAlmacen <= 0)
            throw new NegocioException("Indica un almacen valido");

        var productos = await _repository.ObtenerProductosXAlmacenAsync(idAlmacen);

        return productos.Select(p => new ProductoAlmacenDto
        {
            IdProducto = p.IdProducto,
            Codigo = p.Codigo,
            Nombre = p.Nombre,
            Lote = p.Lote,
            Serie = p.Serie,
            NombreCategoria = p.Categoria?.Nombre ?? string.Empty,
            NombreUnidadMedida = p.Categoria?.Nombre ?? string.Empty,
            CantidadActual = p.StockAlmacen?.Sum(s => s.CantidadActual) ?? 0
        }).ToList();
    }
}
