namespace ProyectoAW.Modules.Seguridad.Security_Session.Permisos.HttpAuth
{
    public interface ICurrentUserService
    {
        int IdUsuario { get; }
        int IdRol { get; }
    }
}
