using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Guias.DTOs.Requests;
using ProyectoAW.Modules.Guias.DTOs.Responses;
using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Guias.Repositories;
using ProyectoAW.Modules.Maestros.Estados.Entities;
using ProyectoAW.Modules.Maestros.MotivosGuia.Entities;
using ProyectoAW.Modules.Maestros.MotivosGuia.Repositories;
using ProyectoAW.Modules.Maestros.Operaciones.Entities;
using ProyectoAW.Modules.Maestros.TipoComportamientos.Entities;
using ProyectoAW.Modules.Seguridad.Security_Session.Permisos.HttpAuth;

namespace ProyectoAW.Modules.Guias.Services;

public class GuiaService : IGuiaService
{
    private readonly IGuiaRepository _repository;
    private readonly IMotivoGuiaRepository _motivoRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly ITransactionManager _transactions;

    public GuiaService(
        IGuiaRepository repository,
        IMotivoGuiaRepository motivoRepository,
        ICurrentUserService currentUser,
        ITransactionManager transactions)
    {
        _repository = repository;
        _motivoRepository = motivoRepository;
        _currentUser = currentUser;
        _transactions = transactions;
    }
    // ConvertirADto eliminar

    public async Task<List<GuiaDto>> ObtenerTodosAsync()
    {
        var guias = await _repository.ObtenerTodosAsync();
        return guias.Select( guia => new GuiaDto
        {
            IdGuia = guia.IdGuia,
            NumeroGuia = guia.NumeroGuia,
            IdTipoOperacionGuia = guia.IdTipoOperacionGuia,
            TipoOperacion = guia.TipoOperacion.Nombre,
            IdMotivoGuia = guia.IdMotivoGuia,
            IdGuiaRelacionada = guia.IdGuiaRelacionada,
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
                CodProducto = detalle.CodProducto,
                Cantidad = detalle.Cantidad
            }).ToList()
        }).ToList();
    }

    public async Task<List<GuiaDto>> ListarGuiasXtipoOperacionAsync(int idTipoOperacionGuia)
    {
        var guiasOperacion = await _repository.ListarGuiasTipoOperacionAsync(idTipoOperacionGuia);
        return guiasOperacion.Select( guia => new GuiaDto
        {
            IdGuia = guia.IdGuia,
            NumeroGuia = guia.NumeroGuia,
            TipoOperacion = guia.TipoOperacion.Nombre,
            IdMotivoGuia = guia.IdMotivoGuia,
            IdGuiaRelacionada = guia.IdGuiaRelacionada,
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
                CodProducto = detalle.CodProducto,
                Cantidad = detalle.Cantidad
            }).ToList()
        }).ToList();
    }


    public async Task<GuiaDto?> ObtenerPorIdAsync(int id)
    {
        var guia = await _repository.ObtenerPorIdAsync(id);

        if (guia == null)
            throw new NegocioException($"No se pudo encontrar la guia con id {id}.");

        return new GuiaDto
        {
            IdGuia = guia.IdGuia,
            NumeroGuia = guia.NumeroGuia,
            IdTipoOperacionGuia = guia.IdTipoOperacionGuia,
            TipoOperacion = guia.TipoOperacion.Nombre,
            IdMotivoGuia = guia.IdMotivoGuia,
            IdGuiaRelacionada = guia.IdGuiaRelacionada,
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
                CodProducto = detalle.CodProducto,
                Cantidad = detalle.Cantidad
            }).ToList()
        };
    }
    

    public async Task<GuiaDto> CrearAsync(CrearGuiaDto request)
    {
        var idUsuarioRegistro = _currentUser.IdUsuario;
        if (idUsuarioRegistro <= 0)
            throw new NegocioException("No se pudo identificar al usuario que registra la guía. Inicia sesión nuevamente.");

        if (!ProyectoAW.Infrastructure.Data.DateTimeText.TryNormalize(request.FechaProgramada, out var fechaProgramada))
            throw new NegocioException("La fecha programada no tiene un formato válido.");

        if (request.IdMotivoGuia <= 0)
            throw new NegocioException("Debe seleccionar un motivo de guía válido.");

        var motivo = await ObtenerMotivoActivoAsync(request.IdMotivoGuia);
        ValidarDatosMotivo(
            motivo,
            request.IdProveedor,
            request.IdCliente,
            request.IdAlmacenOrigen,
            request.IdAlmacenDestino);

        var esTrasladoInterno = motivo.IdTipoComportamiento == TipoComportamientoGuiaIds.TrasladoInterno;
        if (esTrasladoInterno)
            ValidarMotivoTraslado(motivo, request.IdProveedor, request.IdCliente, request.IdAlmacenOrigen, request.IdAlmacenDestino);

        var guia = new Guia
        {
            IdUsuarioRegistro = idUsuarioRegistro,
            NumeroGuia = string.Empty,
            IdTipoOperacionGuia = motivo.IdTipoOperacionGuia,
            IdMotivoGuia = motivo.IdMotivoGuia,
            IdEstadoGuia = EstadoGuiasIds.Pendiente,
            IdProveedor = motivo.RequiereProveedor ? request.IdProveedor : null,
            IdCliente = motivo.RequiereCliente ? request.IdCliente : null,
            IdAlmacenOrigen = motivo.RequiereAlmacenOrigen ? request.IdAlmacenOrigen : null,
            IdAlmacenDestino = motivo.RequiereAlmacenDestino ? request.IdAlmacenDestino : null,
            FechaProgramada = fechaProgramada,
            TipoDocumentoReferencia = request.TipoDocumentoReferencia,
            NumeroDocumentoReferencia = request.NumeroDocumentoReferencia,
            Observacion = request.Observacion?.Trim(),
            Detalles = request.Detalles.Select(detalle => new DetalleGuia
            {
                IdProducto = detalle.IdProducto,
                CodProducto = detalle.CodProducto,
                Cantidad = detalle.Cantidad
            }).ToList()
        };

        await _transactions.ExecuteAsync(async _ =>
        {
            var errorReferencias = await _repository.ValidarReferenciasAsync(guia);
            if (errorReferencias != null)
                throw new NegocioException(errorReferencias);

            await _repository.CrearAsync(guia);
            guia.NumeroGuia = $"{motivo.TipoOperacionGuia!.Codigo}-{guia.IdGuia:D6}";
            // validar si es traslado interno 
            if (esTrasladoInterno)
            {
                var guiaEntrada = new Guia
                {
                    IdUsuarioRegistro = idUsuarioRegistro,
                    IdTipoOperacionGuia = TipoOperacionGuiaIds.Entrada,
                    IdEstadoGuia = EstadoGuiasIds.Pendiente,
                    IdAlmacenOrigen = request.IdAlmacenOrigen,
                    IdAlmacenDestino = request.IdAlmacenDestino,
                    FechaProgramada = fechaProgramada,
                    TipoDocumentoReferencia = request.TipoDocumentoReferencia,
                    NumeroDocumentoReferencia = request.NumeroDocumentoReferencia,
                    Observacion = $"Recepción del traslado {guia.NumeroGuia}. {request.Observacion?.Trim()}".Trim(),
                    Detalles = request.Detalles.Select(detalle => new DetalleGuia
                    {
                        IdProducto = detalle.IdProducto,
                        CodProducto = detalle.CodProducto,
                        Cantidad = detalle.Cantidad
                    }).ToList()
                };

                errorReferencias = await _repository.ValidarReferenciasAsync(guiaEntrada);
                if (errorReferencias != null)
                    throw new NegocioException(errorReferencias);

                await _repository.CrearAsync(guiaEntrada);
                guiaEntrada.NumeroGuia = $"ENT-{guiaEntrada.IdGuia:D6}";
                guiaEntrada.IdGuiaRelacionada = guia.IdGuia;
                guia.IdGuiaRelacionada = guiaEntrada.IdGuia;
                await _repository.ActualizarAsync(guiaEntrada);
                await _repository.ActualizarAsync(guia);
            }
            else
            {
                await _repository.ActualizarAsync(guia);
            }
        });

        var guiaCreada = await _repository.ObtenerPorIdAsync(guia.IdGuia)
            ?? throw new NegocioException("No se pudo recuperar la guía creada.");
        return new GuiaDto
        {
            IdGuia = guiaCreada.IdGuia,
            NumeroGuia = guiaCreada.NumeroGuia,
            TipoOperacion = guiaCreada.TipoOperacion.Nombre,
            IdTipoOperacionGuia = guiaCreada.IdTipoOperacionGuia,
            IdMotivoGuia = guiaCreada.Motivo?.IdMotivoGuia,
            IdGuiaRelacionada = guiaCreada.IdGuiaRelacionada,
            Motivo = guiaCreada.Motivo?.NombreMotivo ?? string.Empty,
            Estado = guiaCreada.Estado.Nombre,
            IdProveedor = guiaCreada.IdProveedor,
            Proveedor = guiaCreada.Proveedor?.RazonSocial,
            IdCliente = guiaCreada.IdCliente,
            Cliente = guiaCreada.Cliente?.RazonSocial,
            IdAlmacenOrigen = guiaCreada.IdAlmacenOrigen,
            AlmacenOrigen = guiaCreada.AlmacenOrigen?.Nombre,
            IdAlmacenDestino = guiaCreada.IdAlmacenDestino,
            AlmacenDestino = guiaCreada.AlmacenDestino?.Nombre,
            FechaProgramada = guiaCreada.FechaProgramada,
            TipoDocumentoReferencia = guiaCreada.TipoDocumentoReferencia,
            NumeroDocumentoReferencia = guiaCreada.NumeroDocumentoReferencia,
            Observacion = guiaCreada.Observacion,
            Detalles = guiaCreada.Detalles.Select(detalle => new DetalleGuiaDto
            {
                IdProducto = detalle.IdProducto,
                Producto = detalle.Producto.Nombre,
                CodProducto = detalle.CodProducto,
                Cantidad = detalle.Cantidad
            }).ToList()
        };
    }
    
    public async Task<bool> ActualizarAsync(int id, ActualizarGuiaDto request)
    {
        if (!ProyectoAW.Infrastructure.Data.DateTimeText.TryNormalize(request.FechaProgramada, out var fechaProgramada))
            throw new NegocioException("La fecha programada no tiene un formato válido.");

        var guia = await _repository.ObtenerPorIdAsync(id);

        if (guia == null)
            throw new NegocioException($"No se encontro la guia con id {id}.");

        if (guia.IdGuiaRelacionada.HasValue)
            throw new NegocioException("Las guías de un traslado interno están vinculadas y no se pueden editar por separado.");

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
        guia.FechaProgramada = fechaProgramada;
        guia.TipoDocumentoReferencia = request.TipoDocumentoReferencia;
        guia.NumeroDocumentoReferencia = request.NumeroDcumentoReferencia;
        guia.Observacion = request.Observacion?.Trim();
        guia.Detalles.Clear();

        foreach (var detalle in request.Detalles)
        {
            guia.Detalles.Add(new DetalleGuia
            {
                IdProducto = detalle.IdProducto,
                Cantidad = detalle.Cantidad,
                CodProducto = detalle.CodProducto
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

        if (guia.IdGuiaRelacionada.HasValue)
        {
            var relacionada = await _repository.ObtenerPorIdAsync(guia.IdGuiaRelacionada.Value)
                ?? throw new NegocioException("No se encontró la guía relacionada al traslado.");
            if (guia.IdEstadoGuia != EstadoGuiasIds.Pendiente || relacionada.IdEstadoGuia != EstadoGuiasIds.Pendiente)
                throw new NegocioException("No se puede cancelar el traslado porque una de sus guías ya fue iniciada o finalizada.");

            await _transactions.ExecuteAsync(async _ =>
            {
                guia.IdEstadoGuia = EstadoGuiasIds.Cancelada;
                relacionada.IdEstadoGuia = EstadoGuiasIds.Cancelada;
                await _repository.ActualizarAsync(guia);
            });
        }
        else
        {
            guia.IdEstadoGuia = EstadoGuiasIds.Cancelada;
            await _repository.ActualizarAsync(guia);
        }
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

    private static void ValidarMotivoTraslado(
        MotivoGuia motivo,
        int? idProveedor,
        int? idCliente,
        int? idAlmacenOrigen,
        int? idAlmacenDestino)
    {
        if (motivo.IdTipoOperacionGuia != TipoOperacionGuiaIds.Salida)
            throw new NegocioException("El motivo de traslado interno debe usar la operación Salida.");

        if (motivo.RequiereProveedor || motivo.RequiereCliente || idProveedor.HasValue || idCliente.HasValue)
            throw new NegocioException("El traslado interno no debe usar proveedor ni cliente.");

        if (!motivo.RequiereAlmacenOrigen || !motivo.RequiereAlmacenDestino ||
            !idAlmacenOrigen.HasValue || !idAlmacenDestino.HasValue)
            throw new NegocioException("El traslado interno requiere almacén de origen y almacén de destino.");

        if (idAlmacenOrigen == idAlmacenDestino)
            throw new NegocioException("El almacén de origen y destino del traslado deben ser diferentes.");
    }
}
