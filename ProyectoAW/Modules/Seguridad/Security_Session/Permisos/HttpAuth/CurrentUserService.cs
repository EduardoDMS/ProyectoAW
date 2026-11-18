using System.Security.Claims;

namespace ProyectoAW.Modules.Seguridad.Security_Session.Permisos.HttpAuth
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccesor;
        
        public CurrentUserService(IHttpContextAccessor httpContextAccesor)
        {
            _httpContextAccesor = httpContextAccesor;
        }

        public int IdUsuario
        {
            get
            {
                var valor = _httpContextAccesor.HttpContext?
                    .User
                    .FindFirst(ClaimTypes.NameIdentifier)
                    ?.Value;

                return int.TryParse(valor, out var id)
                    ? id : 0;
            }
        }

        public int IdRol
        {
            get
            {
                var valor = _httpContextAccesor.HttpContext?
                    .User
                    .FindFirst(ClaimTypes.Role)
                    ?.Value;

                return int.TryParse(valor, out var id)
                ? id : 0;
            }
        }
    }
}
