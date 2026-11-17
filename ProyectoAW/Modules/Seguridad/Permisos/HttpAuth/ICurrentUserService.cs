namespace ProyectoAW.Modules.Seguridad.Permisos.HttpAuth
{
    public interface ICurrentUserService
    {
        int IdUsuario { get; }
        int IdRol { get; }
    }
}
