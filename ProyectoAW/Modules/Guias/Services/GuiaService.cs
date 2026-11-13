using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Modules.Guias.DTOs.Requests;
using ProyectoAW.Modules.Guias.DTOs.Responses;
using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Guias.Repositories;
using ProyectoAW.Modules.Maestros.Estados.Entities;

namespace ProyectoAW.Modules.Guias.Services;

public class GuiaService : IGuiaService
{
    private readonly IGuiaRepository _repository;
    private readonly IMotivoGuiaRepository _motivoRepository;

    public GuiaService(IGuiaRepository repository, IMotivoGuiaRepository motivoRepository)
    {
        _repository = repository;
        _motivoRepository = motivoRepository;
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
            throw new NegocioException($"No se pudo encontrar la guia con id {id}.");

        return ConvertirADto(guia);
    }

    public async Task<GuiaDto> CrearAsync(CrearGuiaDto request)
    {
        var motivo = await ObtenerMotivoActivoAsync(request.IdMotivoGuia);
        ValidarDatosMotivo(
            motivo,
            request.IdProveedor,
            request.IdCliente,
            request.IdAlmacenOrigen,
            request.IdAlmacenDestino);

        var guia = new Guia
        {
            NumeroGuia = string.Empty,
            IdTipoOperacionGuia = motivo.IdTipoOperacionGuia,
            IdMotivoGuia = motivo.IdMotivoGuia,
            IdEstadoGuia = EstadoGuiasIds.Pendiente,
            IdProveedor = motivo.RequiereProveedor ? request.IdProveedor : null,
            IdCliente = motivo.RequiereCliente ? request.IdCliente : null,
            IdAlmacenOrigen = motivo.RequiereAlmacenOrigen ? request.IdAlmacenOrigen : null,
            IdAlmacenDestino = motivo.RequiereAlmacenDestino ? request.IdAlmacenDestino : null,
            FechaProgramada = request.FechaProgramada,
            Observacion = request.Observacion?.Trim(),
            Detalles = request.Detalles.Select(detalle => new DetalleGuia
            {
                IdProducto = detalle.IdProducto,
                Cantidad = detalle.Cantidad
            }).ToList()
        };

        await _repository.CrearAsync(guia);

        guia.NumeroGuia = $"{motivo.TipoOperacionGuia!.Codigo}-{guia.IdGuia:D6}";
        await _repository.ActualizarAsync(guia);

        var guiaCreada = await _repository.ObtenerPorIdAsync(guia.IdGuia);
        return ConvertirADto(guiaCreada!);
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarGuiaDto request)
    {
        var guia = await _repository.ObtenerPorIdAsync(id);

        if (guia == null)
            throw new NegocioException($"No se encontro la guia con id {id}.");

        var motivo = await ObtenerMotivoActivoAsync(request.IdMotivoGuia);
        ValidarDatosMotivo(
            motivo,
            request.IdProveedor,
            request.IdCliente,
            request.IdAlmacenOrigen,
            request.IdAlmacenDestino);

        guia.IdTipoOperacionGuia = motivo.IdTipoOperacionGuia;
        guia.IdMotivoGuia = motivo.IdMotivoGuia;
        guia.NumeroGuia = $"{motivo.TipoOperacionGuia!.Codigo}-{guia.IdGuia:D6}";
        guia.IdProveedor = motivo.RequiereProveedor ? request.IdProveedor : null;
        guia.IdCliente = motivo.RequiereCliente ? request.IdCliente : null;
        guia.IdAlmacenOrigen = motivo.RequiereAlmacenOrigen ? request.IdAlmacenOrigen : null;
        guia.IdAlmacenDestino = motivo.RequiereAlmacenDestino ? request.IdAlmacenDestino : null;
        guia.FechaProgramada = request.FechaProgramada;
        guia.Observacion = request.Observacion?.Trim();
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
            throw new NegocioException($"No se encontro la guia con id {id}.");

        guia.IdEstadoGuia = EstadoGuiasIds.Cancelada;
        await _repository.ActualizarAsync(guia);
        return true;
    }

    private async Task<MotivoGuia> ObtenerMotivoActivoAsync(int id)
    {
        var motivo = await _motivoRepository.ObtenerAsync(id);

        if (motivo == null || !motivo.Activo)
            throw new NegocioException("El motivo seleccionado no existe o esta inactivo.");

        return motivo;
    }

    private void ValidarDatosMotivo(
        MotivoGuia motivo,
        int? idProveedor,
        int? idCliente,
        int? idAlmacenOrigen,
        int? idAlmacenDestino)
    {
        if (motivo.RequiereProveedor && (!idProveedor.HasValue || idProveedor <= 0))
            throw new NegocioException("El motivo seleccionado requiere un proveedor.");

        if (motivo.RequiereCliente && (!idCliente.HasValue || idCliente <= 0))
            throw new NegocioException("El motivo seleccionado requiere un cliente.");

        if (motivo.RequiereAlmacenOrigen && (!idAlmacenOrigen.HasValue || idAlmacenOrigen <= 0))
            throw new NegocioException("El motivo seleccionado requiere un almacen de origen.");

        if (motivo.RequiereAlmacenDestino && (!idAlmacenDestino.HasValue || idAlmacenDestino <= 0))
            throw new NegocioException("El motivo seleccionado requiere un almacen de destino.");
    }

    private GuiaDto ConvertirADto(Guia guia)
    {
        return new GuiaDto
        {
            IdGuia = guia.IdGuia,
            NumeroGuia = guia.NumeroGuia,
            TipoOperacion = guia.TipoOperacion.Nombre,
            IdMotivoGuia = guia.IdMotivoGuia,
            Motivo = guia.Motivo?.NombreMotivo ?? string.Empty,
            Estado = guia.Estado.Nombre,
            IdProveedor = guia.IdProveedor,
            Proveedor = guia.Proveedor?.RazonSocial,
            IdCliente = guia.IdCliente,
            Cliente = guia.Cliente?.RazonSocial,
            IdAlmacenOrigen = guia.IdAlmacenOrigen,
            AlmacenOrigen = guia.AlmacenOrigen?.Nombre,
            IdAlmacenDestino = guia.IdAlmacenDestino,
            AlmacenDestino = guia.AlmacenDestino?.Nombre,
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
