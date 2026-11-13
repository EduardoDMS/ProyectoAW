using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Modules.Guias.DTOs.Requests;
using ProyectoAW.Modules.Guias.DTOs.Responses;
using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Guias.Repositories;

namespace ProyectoAW.Modules.Guias.Services;

public class MotivoGuiaService : IMotivoGuiaService
{
    private readonly IMotivoGuiaRepository _repository;

    public MotivoGuiaService(IMotivoGuiaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<MotivoGuiaDto>> ListarAsync(int? idOperacion, bool soloActivos)
    {
        var motivos = await _repository.ListarAsync(idOperacion, soloActivos);

        return motivos.Select(motivo => new MotivoGuiaDto
        {
            IdMotivoGuia = motivo.IdMotivoGuia,
            IdTipoOperacionGuia = motivo.IdTipoOperacionGuia,
            TipoOperacion = motivo.TipoOperacionGuia?.Nombre ?? string.Empty,
            NombreMotivo = motivo.NombreMotivo,
            RequiereProveedor = motivo.RequiereProveedor,
            RequiereCliente = motivo.RequiereCliente,
            RequiereAlmacenOrigen = motivo.RequiereAlmacenOrigen,
            RequiereAlmacenDestino = motivo.RequiereAlmacenDestino,
            IdTipoComportamiento = motivo.IdTipoComportamiento,
            TipoComportamiento = motivo.TipoComportamiento?.Nombre ?? string.Empty,
            Activo = motivo.Activo
        }).ToList();
    }

    public async Task<MotivoGuiaDto> ObtenerAsync(int id)
    {
        var motivo = await _repository.ObtenerAsync(id);

        if (motivo == null)
            throw new NegocioException("No se encontro el motivo de guia.");

        return new MotivoGuiaDto
        {
            IdMotivoGuia = motivo.IdMotivoGuia,
            IdTipoOperacionGuia = motivo.IdTipoOperacionGuia,
            TipoOperacion = motivo.TipoOperacionGuia?.Nombre ?? string.Empty,
            NombreMotivo = motivo.NombreMotivo,
            RequiereProveedor = motivo.RequiereProveedor,
            RequiereCliente = motivo.RequiereCliente,
            RequiereAlmacenOrigen = motivo.RequiereAlmacenOrigen,
            RequiereAlmacenDestino = motivo.RequiereAlmacenDestino,
            IdTipoComportamiento = motivo.IdTipoComportamiento,
            TipoComportamiento = motivo.TipoComportamiento?.Nombre ?? string.Empty,
            Activo = motivo.Activo
        };
    }

    public async Task<List<TipoComportamientoGuiaDto>> ListarComportamientosAsync()
    {
        var tipos = await _repository.ListarComportamientosAsync();

        return tipos.Select(tipo => new TipoComportamientoGuiaDto
        {
            IdTipoComportamiento = tipo.IdTipoComportamiento,
            Nombre = tipo.Nombre,
            Descripcion = tipo.Descripcion
        }).ToList();
    }

    public async Task<MotivoGuiaDto> CrearAsync(CrearMotivoGuiaDto dto)
    {
        var nombre = dto.NombreMotivo.Trim();

        if (!await _repository.ExisteOperacionAsync(dto.IdTipoOperacionGuia))
            throw new NegocioException("La operacion seleccionada no existe.");

        if (!await _repository.ExisteComportamientoAsync(dto.IdTipoComportamiento))
            throw new NegocioException("El tipo de comportamiento seleccionado no existe.");

        if (await _repository.NombreExisteAsync(dto.IdTipoOperacionGuia, nombre))
            throw new NegocioException("Ya existe un motivo con ese nombre para la operacion.");

        var motivo = new MotivoGuia
        {
            IdTipoOperacionGuia = dto.IdTipoOperacionGuia,
            NombreMotivo = nombre,
            RequiereProveedor = dto.RequiereProveedor,
            RequiereCliente = dto.RequiereCliente,
            RequiereAlmacenOrigen = dto.RequiereAlmacenOrigen,
            RequiereAlmacenDestino = dto.RequiereAlmacenDestino,
            IdTipoComportamiento = dto.IdTipoComportamiento,
            Activo = true
        };

        await _repository.AgregarAsync(motivo);
        return await ObtenerAsync(motivo.IdMotivoGuia);
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarMotivoGuiaDto dto)
    {
        var motivo = await _repository.ObtenerAsync(id);

        if (motivo == null)
            throw new NegocioException("No se encontro el motivo de guia.");

        var nombre = dto.NombreMotivo.Trim();

        if (!await _repository.ExisteOperacionAsync(dto.IdTipoOperacionGuia))
            throw new NegocioException("La operacion seleccionada no existe.");

        if (!await _repository.ExisteComportamientoAsync(dto.IdTipoComportamiento))
            throw new NegocioException("El tipo de comportamiento seleccionado no existe.");

        if (await _repository.NombreExisteAsync(dto.IdTipoOperacionGuia, nombre, id))
            throw new NegocioException("Ya existe un motivo con ese nombre para la operacion.");

        motivo.IdTipoOperacionGuia = dto.IdTipoOperacionGuia;
        motivo.NombreMotivo = nombre;
        motivo.RequiereProveedor = dto.RequiereProveedor;
        motivo.RequiereCliente = dto.RequiereCliente;
        motivo.RequiereAlmacenOrigen = dto.RequiereAlmacenOrigen;
        motivo.RequiereAlmacenDestino = dto.RequiereAlmacenDestino;
        motivo.IdTipoComportamiento = dto.IdTipoComportamiento;
        motivo.Activo = dto.Activo;

        await _repository.ActualizarAsync(motivo);
        return true;
    }

    public async Task<bool> CambiarEstadoAsync(int id, bool activo)
    {
        var motivo = await _repository.ObtenerAsync(id);

        if (motivo == null)
            throw new NegocioException("No se encontro el motivo de guia.");

        motivo.Activo = activo;
        await _repository.ActualizarAsync(motivo);
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var motivo = await _repository.ObtenerAsync(id);

        if (motivo == null)
            throw new NegocioException("No se encontro el motivo de guia.");

        if (await _repository.EstaEnUsoAsync(id))
            throw new NegocioException("No se puede eliminar el motivo porque ya esta asociado a una guia.");

        _repository.Eliminar(motivo);
        await _repository.GuardarAsync();
        return true;
    }
}
