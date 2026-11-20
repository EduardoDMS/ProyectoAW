using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Modules.Seguridad.Perfiles.Dtos.Request;
using ProyectoAW.Modules.Seguridad.Perfiles.Dtos.Response;
using ProyectoAW.Modules.Seguridad.Perfiles.Entites;
using ProyectoAW.Modules.Seguridad.Perfiles.Repositories;

namespace ProyectoAW.Modules.Seguridad.Perfiles.Services;

public class PerfilService : IPerfilService
{
    private readonly IPerfilRepository _repository;

    public PerfilService(IPerfilRepository repository)
    {
        _repository = repository;
    }

    public Task<List<string>> ListarModulosAsync()
    {
        return Task.FromResult(CatalogoPermisos.ListarModulos());
    }

    public async Task<List<PerfilDto>> ListarAsync()
    {
        var perfiles = await _repository.ListarAsync();
        return perfiles.Select(ConvertirADto).ToList();
    }

    public async Task<PerfilDto> ObtenerPorIdAsync(int id)
    {
        var perfil = await _repository.ObtenerPorIdAsync(id);

        if (perfil == null)
            throw new NegocioException($"No se encontró el perfil con ID {id}.");

        return ConvertirADto(perfil);
    }

    public async Task<PerfilDto> CrearAsync(CrearPerfilDto request)
    {
        var nombre = request.NombrePerfil.Trim();
        ValidarNombre(nombre);
        ValidarPermisos(request.Permisos);

        if (await _repository.NombreExisteAsync(nombre))
            throw new NegocioException("Ya existe un perfil con ese nombre.");

        var perfil = new Perfil
        {
            NombrePerfil = nombre,
            Permisos = request.Permisos.Select(CrearPermiso).ToList()
        };

        await _repository.CrearAsync(perfil);
        return ConvertirADto(perfil);
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarPerfilDto request)
    {
        var perfil = await _repository.ObtenerPorIdAsync(id);

        if (perfil == null)
            throw new NegocioException($"No se encontró el perfil con ID {id}.");

        var nombre = request.NombrePerfil.Trim();
        ValidarNombre(nombre);
        ValidarPermisos(request.Permisos);

        if (await _repository.NombreExisteAsync(nombre, id))
            throw new NegocioException("Ya existe un perfil con ese nombre.");

        perfil.NombrePerfil = nombre;
        perfil.Permisos.Clear();

        foreach (var permiso in request.Permisos)
            perfil.Permisos.Add(CrearPermiso(permiso));

        await _repository.ActualizarAsync(perfil);
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var perfil = await _repository.ObtenerPorIdAsync(id);

        if (perfil == null)
            throw new NegocioException($"No se encontró el perfil con ID {id}.");

        if (await _repository.TieneUsuariosAsignadosAsync(id))
            throw new NegocioException("No se puede eliminar el perfil porque está asignado a uno o más usuarios.");

        await _repository.EliminarAsync(perfil);
        return true;
    }

    private static void ValidarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new NegocioException("Ingresa el nombre del perfil.");

        if (nombre.Length > 100)
            throw new NegocioException("El nombre del perfil no puede superar los 100 caracteres.");
    }

    private static void ValidarPermisos(List<PermisoPerfilRequestDto> permisos)
    {
        var modulos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var permiso in permisos)
        {
            var modulo = permiso.Modulo?.Trim();

            if (string.IsNullOrWhiteSpace(modulo))
                throw new NegocioException("Todos los permisos deben indicar un módulo.");

            if (!CatalogoPermisos.ExisteModulo(modulo))
                throw new NegocioException($"El módulo '{modulo}' no existe en el catálogo de permisos.");

            if (!modulos.Add(modulo))
                throw new NegocioException($"El módulo '{modulo}' está repetido en los permisos.");
        }
    }

    private static PermisoPerfil CrearPermiso(PermisoPerfilRequestDto request)
    {
        return new PermisoPerfil
        {
            Modulo = CatalogoPermisos.ObtenerModulo(request.Modulo),
            Ver = request.Ver,
            Crear = request.Crear,
            Actualizar = request.Actualizar,
            Eliminar = request.Eliminar
        };
    }

    private static PerfilDto ConvertirADto(Perfil perfil)
    {
        return new PerfilDto
        {
            IdPerfil = perfil.IdPerfil,
            NombrePerfil = perfil.NombrePerfil,
            Permisos = perfil.Permisos.Select(permiso => new PermisoPerfilDto
            {
                Modulo = permiso.Modulo,
                Ver = permiso.Ver,
                Crear = permiso.Crear,
                Actualizar = permiso.Actualizar,
                Eliminar = permiso.Eliminar
            }).ToList()
        };
    }
}
