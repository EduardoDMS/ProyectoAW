using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoAW.Modules.Maestros.UnidadesMedida.Controllers;

[Route("Maestros/UnidadesMedida")]
    [Authorize(Policy = "UnidadesMedida.Ver")]
public class UnidadMedidaController : Controller
{
    [HttpGet("")]
    public IActionResult Index() { RouteData.Values["viewModule"] = "Maestros/UnidadesMedida"; return View(); }
}
