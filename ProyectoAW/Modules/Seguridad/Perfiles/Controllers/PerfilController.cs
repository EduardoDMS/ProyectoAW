using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoAW.Modules.Seguridad.Perfiles.Controllers;

[Authorize(Policy = "Administrador")]
[Route("Seguridad/Perfiles")]
public class PerfilController : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        RouteData.Values["viewModule"] = "Seguridad/Perfiles";
        return View();
    }
}
