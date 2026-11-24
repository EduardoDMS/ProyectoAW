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

    public async Task<List<string>> ListarModulosAsync(int idRol)
    {
        var nombreRol = await ObtenerNombreRolValidoAsync(idRol);
        return CatalogoPermisos.ListarModulosPermitidos(nombreRol);
    }

    public async Task<List<PerfilDto>> ListarAsync()
    {
        var perfiles = await _repository.ListarAsync();
        return perfiles.Select(perfil => new PerfilDto
        {
            IdPerfil = perfil.IdPerfil,
            NombrePerfil = perfil.NombrePerfil,
            IdRol = perfil.IdRol,
            NombreRol = perfil.rol.Nombre,
            Activo = perfil.Activo,
            Permisos = perfil.Permisos.Select(permiso => new PermisoPerfilDto
            {
                Modulo = permiso.Modulo,
                Ver = permiso.Ver,
                Crear = permiso.Crear,
                Actualizar = permiso.Actualizar,
                Eliminar = permiso.Eliminar,
            }).ToList()
        }).ToList();
    }

    public async Task<PerfilDto> ObtenerPorIdAsync(int id)
    {
        var perfil = await _repository.ObtenerPorIdAsync(id);

        if (perfil == null)
            throw new NegocioException($"No se encontró el perfil con ID {id}.");

        return new PerfilDto
        {
            IdPerfil = perfil.IdPerfil,
            NombrePerfil = perfil.NombrePerfil,
            IdRol = perfil.IdRol,
            NombreRol = perfil.rol.Nombre,
            Activo = perfil.Activo,
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

    public async Task<PerfilDto> CrearAsync(CrearPerfilDto request)
    {
        var nombre = request.NombrePerfil.Trim();
        ValidarNombre(nombre);
        var nombreRol = await ObtenerNombreRolValidoAsync(request.IdRol);
        ValidarPermisos(request.Permisos, nombreRol);

        if (await _repository.NombreExisteAsync(nombre))
            throw new NegocioException("Ya existe un perfil con ese nombre.");

        var perfil = new Perfil
        {
            NombrePerfil = nombre,
            IdRol = request.IdRol,
            Permisos = request.Permisos.Select(CrearPermiso).ToList()
        };

        await _repository.CrearAsync(perfil);
        return new PerfilDto
        {
            IdPerfil = perfil.IdPerfil,
            NombrePerfil = perfil.NombrePerfil,
            IdRol = perfil.IdRol,
            NombreRol = perfil.rol.Nombre,
            Activo = perfil.Activo,
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

    public async Task<bool> ActualizarAsync(int id, ActualizarPerfilDto request)
    {
        var perfil = await _repository.ObtenerPorIdAsync(id);
        if (perfil == null)
            throw new NegocioException($"No se encontró el perfil con ID {id}.");


        var nombre = request.NombrePerfil.Trim();
        ValidarNombre(nombre);

        if (await _repository.NombreExisteAsync(nombre, id))
            throw new NegocioException("Ya existe un perfil con ese nombre.");

        var nombreRol = await ObtenerNombreRolValidoAsync(request.IdRol);
        ValidarPermisos(request.Permisos, nombreRol);


        perfil.NombrePerfil = nombre;
        perfil.IdRol = request.IdRol;
        perfil.Activo = request.Activo;
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

    private async Task<string> ObtenerNombreRolValidoAsync(int idRol)
    {
        var nombreRol = await _repository.ObtenerNombreRolAsync(idRol);

        if (string.IsNullOrWhiteSpace(nombreRol))
            throw new NegocioException("No se encontró el rol indicado.");

        if (!CatalogoPermisos.EsAdministrador(nombreRol) && !CatalogoPermisos.EsOperador(nombreRol))
            throw new NegocioException("Solo se pueden crear perfiles para los roles Administrador u Operador.");

        return nombreRol;
    }

    private static void ValidarPermisos(List<PermisoPerfilRequestDto> permisos, string nombreRol)
    {
        var modulos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var modulosPermitidos = new HashSet<string>(
            CatalogoPermisos.ListarModulosPermitidos(nombreRol),
            StringComparer.OrdinalIgnoreCase);

        foreach (var permiso in permisos)
        {
            var modulo = permiso.Modulo?.Trim();

            if (string.IsNullOrWhiteSpace(modulo))
                throw new NegocioException("Todos los permisos deben indicar un módulo.");

            if (!CatalogoPermisos.ExisteModulo(modulo))
                throw new NegocioException($"El módulo '{modulo}' no existe en el catálogo de permisos.");

            if (!modulosPermitidos.Contains(modulo))
                throw new NegocioException($"El rol '{nombreRol}' no puede tener permisos para el módulo '{modulo}'.");

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
}
