using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Modules.Guias.DTOs.Requests;
using ProyectoAW.Modules.Guias.DTOs.Responses;
using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Guias.Repositories;
using ProyectoAW.Modules.Maestros.Estados.Entities;
using ProyectoAW.Modules.Maestros.Operaciones.Entities;

namespace ProyectoAW.Modules.Guias.Services;

public class GuiaService : IGuiaService
{
    private readonly IGuiaRepository _repository;

    public GuiaService(IGuiaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<GuiaDto>> ObtenerTodosAsync()
    {
        var guias = await _repository.ObtenerTodosAsync();

        return guias.Select(g => new GuiaDto
        {
            IdGuia = g.IdGuia,
            NumeroGuia = g.NumeroGuia,
            TipoOperacion = g.TipoOperacion.Nombre,
            Estado = g.Estado.Nombre,
            IdProveedor = g.IdProveedor,
            Proveedor = g.Proveedor.RazonSocial,
            FechaProgramada = g.FechaProgramada,
            Observacion = g.Observacion,
            Detalles = g.Detalles.Select(d => new DetalleGuiaDto
            {
                IdProducto = d.IdProducto,
                Producto = d.Producto.Nombre,
                Cantidad = d.Cantidad
            }).ToList()
        }).ToList();
    }

    public async Task<GuiaDto?> ObtenerPorIdAsync(int id)
    {
        var guia = await _repository.ObtenerPorIdAsync(id);

        if (guia == null)
            throw new NegocioException($"No se pudo encontrar la guia con id de {id}");

        return new GuiaDto
        {
            IdGuia = guia.IdGuia,
            NumeroGuia = guia.NumeroGuia,
            TipoOperacion = guia.TipoOperacion.Nombre,
            Estado = guia.Estado.Nombre,
            IdProveedor = guia.IdProveedor,
            Proveedor = guia.Proveedor.RazonSocial,
            FechaProgramada = guia.FechaProgramada,
            Observacion = guia.Observacion,
            Detalles = guia.Detalles.Select(d => new DetalleGuiaDto
            {
                IdProducto = d.IdProducto,
                Producto = d.Producto.Nombre,
                Cantidad = d.Cantidad
            }).ToList()
        };
    }

    public async Task<GuiaDto> CrearAsync(CrearGuiaDto request)
    {
        var guia = new Guia
        {
            NumeroGuia = string.Empty,
            IdTipoOperacionGuia = TipoOperacionGuiaIds.Entrada,
            IdEstadoGuia = EstadoGuiasIds.Pendiente,
            IdProveedor = request.IdProveedor,
            FechaProgramada = request.FechaProgramada,
            Observacion = request.Observacion,
            Detalles = request.Detalles.Select(detalle => new DetalleGuia
            {
                IdProducto = detalle.IdProducto,
                Cantidad = detalle.Cantidad
            }).ToList()
        };

        await _repository.CrearAsync(guia);

        // El ID lo genera la base de datos; con él armamos un número sencillo para mostrar.
        guia.NumeroGuia = $"ENT-{guia.IdGuia:D6}";
        await _repository.ActualizarAsync(guia);

        // Se vuelve a consultar para traer TipoOperacion, Estado, Proveedor y Producto,
        // que no están cargados en la entidad recién creada.
        var guiaCreada = await _repository.ObtenerPorIdAsync(guia.IdGuia);

        return new GuiaDto
        {
            IdGuia = guiaCreada!.IdGuia,
            NumeroGuia = guiaCreada.NumeroGuia,
            TipoOperacion = guiaCreada.TipoOperacion.Nombre,
            Estado = guiaCreada.Estado.Nombre,
            IdProveedor = guiaCreada.IdProveedor,
            Proveedor = guiaCreada.Proveedor.RazonSocial,
            FechaProgramada = guiaCreada.FechaProgramada,
            Observacion = guiaCreada.Observacion,
            Detalles = guiaCreada.Detalles.Select(d => new DetalleGuiaDto
            {
                IdProducto = d.IdProducto,
                Producto = d.Producto.Nombre,
                Cantidad = d.Cantidad
            }).ToList()
        };
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarGuiaDto request)
    {
        var guia = await _repository.ObtenerPorIdAsync(id);

        if (guia == null)
            throw new NegocioException($"No se encontro la guia con el id de {id}");
        
        guia.IdProveedor = request.IdProveedor;
        guia.FechaProgramada = request.FechaProgramada;
        guia.Observacion = request.Observacion;
        guia.Detalles.Clear();

        foreach (var detalle in request.Detalles)
        {
            guia.Detalles.Add(new DetalleGuia
            {
                IdProducto = detalle.IdProducto,
                Cantidad = detalle.Cantidad
            });
        }

        await _repository.ActualizarAsync(guia);
        return true;
    }

    public async Task<bool> CancelarAsync(int id)
    {
        var guia = await _repository.ObtenerPorIdAsync(id);

        if (guia == null)
            throw new NegocioException($"No se encontro la guia con el id solicitado: {id}");

        guia.IdEstadoGuia = EstadoGuiasIds.Cancelada;
        await _repository.ActualizarAsync(guia);
        return true;
    }
}