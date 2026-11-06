using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using ProyectoAW.Modules.Seguridad.Autenticacion;

namespace ProyectoAW.Modules.Seguridad.Usuarios.Controllers;

[Route("Maestros/Usuario")]
//[Authorize(Policy = SeguridadPolicies.Admin)]
public class UsuarioController : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        RouteData.Values["viewModule"] = "Seguridad/Usuarios";
        return View();
    }
}
