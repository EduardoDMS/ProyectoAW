using Microsoft.AspNetCore.Identity;
using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Modules.Seguridad.Roles.Services;
using ProyectoAW.Modules.Seguridad.Perfiles.Services;
using ProyectoAW.Modules.Seguridad.Usuarios.DTOs.Requests;
using ProyectoAW.Modules.Seguridad.Usuarios.DTOs.Responses;
using ProyectoAW.Modules.Seguridad.Usuarios.Entitites;
using ProyectoAW.Modules.Seguridad.Usuarios.Repositories;
using ProyectoAW.Modules.Seguridad.Usuarios.Services;

namespace ProyectoAW.Modules.Seguridad.Usuarios.Services;

public class UsuarioService : IUsuarioService
{
    private static readonly string[] RolesAdministrables =
    [
        "Operador",
        "Administrador",
        "Administrador inicial",
        "Admin"
    ];

    private readonly IUsuarioRepository _repository;
    private readonly IRolService _rolService;
    private readonly IPerfilService _perfilService;

    public UsuarioService(
        IUsuarioRepository repository,
        IRolService rolService,
        IPerfilService perfilService)
    {
        _repository = repository;
        _rolService = rolService;
        _perfilService = perfilService;
    }

    public async Task<bool> ActualizarUsuarioAsync(int id, ActualizarUsuarioDto dto)
    {
        ValidarDatos(dto.Nombre, dto.Apellido, dto.Username);
        var usuario = await _repository.GetByIdAsync(id);
        if (usuario is null) 
            throw new NegocioException("Usuario no encontrado.");

        // Confirma que el rol sea asignable y que el nombre de usuario no pertenezca a otra cuenta.
        await ValidarRolAsync(dto.IdRol);
        await ValidarPerfilAsync(dto.IdPerfilUsuario, dto.IdRol);
        var username = dto.Username.Trim();
        var usuarioConElmismoNombre = await _repository.ObtenerPorUsernameAsync(username);
        if (usuarioConElmismoNombre is not null && usuarioConElmismoNombre.IdUsuario != id)
            throw new NegocioException("Ya existe un usuario con ese nombre de usuario.");

        usuario.Username = username;
        usuario.Nombre = dto.Nombre.Trim();
        usuario.Apellido = dto.Apellido.Trim();
        usuario.IdRol = dto.IdRol;
        usuario.IdPerfilUsuario = dto.IdPerfilUsuario;
        usuario.Activo = dto.Activo;
        usuario.FechaActualizada = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);
        usuario.Password = dto.Password;
        // Una contraseña vacía significa conservar la actual; si llega una nueva, se guarda hasheada.
  //

        await _repository.ActualizarAsync(usuario);
        return true;
    }

    public async Task<bool> CambiarEstadoAsync(int id, bool activo)
    {
        var usuario = await _repository.GetByIdAsync(id);
        if (usuario is null) 
            throw new NegocioException("Usuario no encontrado.");
        usuario.Activo = activo;
        await _repository.ActualizarAsync(usuario);
        return true;
    }

    public async Task<UsuarioDto?> CrearUsuarioAsync(CrearUsuarioDto dto)
    {
        ValidarDatos(dto.Nombre, dto.Apellido, dto.Username);
        if (string.IsNullOrWhiteSpace(dto.Password))
            throw new NegocioException("La contraseña es obligatoria.");

        await ValidarRolAsync(dto.IdRol);
        await ValidarPerfilAsync(dto.IdPerfilUsuario, dto.IdRol);
        var username = dto.Username.Trim();
        if (await _repository.ObtenerPorUsernameAsync(username) is not null)
            throw new NegocioException("Ya existe un usuario con ese nombre de usuario.");

        var usuario = new Usuario
        {
            Username = username,
            Nombre = dto.Nombre.Trim(),
            Apellido = dto.Apellido.Trim(),
            IdRol = dto.IdRol,
            IdPerfilUsuario = dto.IdPerfilUsuario,
            Activo = dto.Activo,
            FechaRegistro = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now),
            Password = dto.Password
        };
       //
        await _repository.CrearAsync(usuario);
        return await ObtenerUsuarioPorIdAsync(usuario.IdUsuario);
    }

    public async Task<List<UsuarioDto>> ListarUsuariosAsync() =>
        (await _repository.GetAllAsync()).Select(ToDto).ToList();

    public async Task<UsuarioDto?> ObtenerUsuarioPorIdAsync(int id)
    {
        var usuario = await _repository.GetByIdAsync(id);
        return usuario is null ? null : ToDto(usuario);
    }

    private async Task ValidarRolAsync(int idRol)
    {
        var rol = await _rolService.ObtenerRolPorIdAsync(idRol);
        if (rol is null)
            throw new NegocioException("Selecciona un rol válido.");

        var nombre = rol.Nombre.Trim();
        var permitido = RolesAdministrables.Any(rolPermitido =>
            nombre.Equals(rolPermitido, StringComparison.OrdinalIgnoreCase));
        if (!permitido)
            throw new NegocioException("Solo se puede asignar el rol Administrador o Operador.");
    }

    private async Task ValidarPerfilAsync(int idPerfilUsuario, int idRol)
    {
        if (idPerfilUsuario <= 0)
            throw new NegocioException("Selecciona un perfil válido.");

        var perfil = await _perfilService.ObtenerPorIdAsync(idPerfilUsuario);
        if (perfil.IdRol != idRol)
            throw new NegocioException("El perfil seleccionado no corresponde al rol del usuario.");
    }

    private static void ValidarDatos(string nombre, string apellido, string username)
    {
        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido) ||
            string.IsNullOrWhiteSpace(username))
            throw new NegocioException("Nombre, apellido y nombre de usuario son obligatorios.");
        if (nombre.Trim().Length > 100 || apellido.Trim().Length > 100 || username.Trim().Length > 20)
            throw new NegocioException("El nombre y apellido admiten hasta 100 caracteres; el usuario, hasta 20.");
    }

    private static UsuarioDto ToDto(Usuario usuario) => new()
    {
        IdUsuario = usuario.IdUsuario,
        Nombre = usuario.Nombre,
        Apellido = usuario.Apellido,
        Username = usuario.Username,
        Password = usuario.Password,
        IdRol = usuario.IdRol,
        NombreRol = usuario.rol?.Nombre ?? string.Empty,
        IdPerfilUsuario = usuario.IdPerfilUsuario,
        NombrePerfil = usuario.PerfilUsuario?.NombrePerfil ?? string.Empty,
        Activo = usuario.Activo
    };
}
