using System.Security.Claims;
using ProyectoAW.Modules.Seguridad.Perfiles.Entites;

namespace ProyectoAW.Modules.Seguridad.Perfiles.Services;

public static class CatalogoPermisos
{
    private static readonly string[] Modulos =
    {
        "Almacenes",
        "Categorias",
        "Clientes",
        "Productos",
        "Proveedores",
        "TipoDocumentos",
        "Ubicaciones",
        "UnidadesMedida",
        "Guias",
        "MotivosGuia",
        "Stocks",
        "Usuarios",
        "Roles",
        "Perfiles",
        "TipoComportamiento"
    };

    private static readonly string[] Acciones =
    {
        "Ver",
        "Crear",
        "Actualizar",
        "Eliminar"
    };

    private static readonly string[] ModulosOperador =
    {
        "Guias",
        "Stocks",
        "Usuarios"
    };

    public static List<string> ListarModulos() => Modulos.ToList();

    public static List<string> ListarModulosPermitidos(string? nombreRol)
    {
        if (EsAdministrador(nombreRol))
            return ListarModulos();

        if (EsOperador(nombreRol))
            return ModulosOperador.ToList();

        return new List<string>();
    }

    public static List<string> ListarCodigosPermiso()
    {
        return Modulos
            .SelectMany(modulo => Acciones.Select(accion => $"{modulo}.{accion}"))
            .ToList();
    }

    public static bool ExisteModulo(string? modulo) =>
        !string.IsNullOrWhiteSpace(modulo) &&
        Modulos.Contains(modulo.Trim(), StringComparer.OrdinalIgnoreCase);

    public static string ObtenerModulo(string modulo) =>
        Modulos.First(valor => valor.Equals(modulo.Trim(), StringComparison.OrdinalIgnoreCase));

    public static bool EsAdministrador(string? nombreRol) =>
        nombreRol != null &&
        (nombreRol.Equals("Administrador", StringComparison.OrdinalIgnoreCase) ||
         nombreRol.Equals("Administrador inicial", StringComparison.OrdinalIgnoreCase) ||
         nombreRol.Equals("Admin", StringComparison.OrdinalIgnoreCase));

    public static bool EsOperador(string? nombreRol) =>
        nombreRol != null &&
        nombreRol.Trim().Equals("Operador", StringComparison.OrdinalIgnoreCase);

    public static bool EsAdministrador(ClaimsPrincipal usuario) =>
        usuario.IsInRole("Administrador") ||
        usuario.IsInRole("Administrador inicial") ||
        usuario.IsInRole("Admin");

    public static List<string> ObtenerPermisos(IEnumerable<PermisoPerfil> permisos)
    {
        var codigos = new List<string>();

        foreach (var permiso in permisos)
        {
            var modulo = ObtenerModulo(permiso.Modulo);

            if (permiso.Ver)
                codigos.Add($"{modulo}.Ver");
            if (permiso.Crear)
                codigos.Add($"{modulo}.Crear");
            if (permiso.Actualizar)
                codigos.Add($"{modulo}.Actualizar");
            if (permiso.Eliminar)
                codigos.Add($"{modulo}.Eliminar");
        }

        return codigos;
    }

    public static List<string> ObtenerPermisos(string? nombreRol, IEnumerable<PermisoPerfil> permisos)
    {
        if (EsAdministrador(nombreRol))
            return ListarCodigosPermiso();

        if (!EsOperador(nombreRol))
            return new List<string>();

        var modulosPermitidos = new HashSet<string>(ModulosOperador, StringComparer.OrdinalIgnoreCase);
        return ObtenerPermisos(permisos.Where(permiso => modulosPermitidos.Contains(permiso.Modulo)));
    }
}
