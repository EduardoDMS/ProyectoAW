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
        return guias.Select(ConvertirADto).ToList();
    }

    public async Task<GuiaDto?> ObtenerPorIdAsync(int id)
    {
        var guia = await _repository.ObtenerPorIdAsync(id);

        if (guia == null)
            return null;

        return ConvertirADto(guia);
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

        return (await ObtenerPorIdAsync(guia.IdGuia))!;
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarGuiaDto request)
    {
        var guia = await _repository.ObtenerPorIdAsync(id);

        if (guia == null)
            return false;

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
            return false;

        guia.IdEstadoGuia = EstadoGuiasIds.Cancelada;
        await _repository.ActualizarAsync(guia);
        return true;
    }

    private static GuiaDto ConvertirADto(Guia guia)
    {
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
            Detalles = guia.Detalles.Select(detalle => new DetalleGuiaDto
            {
                IdProducto = detalle.IdProducto,
                Producto = detalle.Producto.Nombre,
                Cantidad = detalle.Cantidad
            }).ToList()
        };
    }
}
