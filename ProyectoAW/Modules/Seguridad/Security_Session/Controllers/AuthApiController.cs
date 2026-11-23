using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAW.Modules.Seguridad.Security_Session.DTOs.Request;
using ProyectoAW.Modules.Seguridad.Security_Session.Services;

namespace ProyectoAW.Modules.Seguridad.Security_Session.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IJwtService _jwtService;

        public AuthApiController(IAuthService service, IJwtService jwtService)
        {
            _service = service;
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var sesion = await _service.ValidarCredencialAsync(request);
            if(sesion is null)
            {
                return Problem(detail: "Usuario o Contraseña Incorrectos.",
                               statusCode: 401,
                               title: "Credenciales Invalidas");
            }

            var token = _jwtService.GeneracionToken(sesion);

            return Ok(new
            {
                token,
                usuario = sesion
            });
        }




    }
}
