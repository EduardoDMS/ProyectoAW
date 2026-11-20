using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoAW.Modules.Maestros.Ubicaciones.Controllers
{
    [Route("Maestros/Ubicacion")]
    [Authorize(Policy = "Ubicaciones.Ver")]
    public class UbicacionController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            RouteData.Values["viewModule"] = "Maestros/Ubicaciones";
            return View();
        }

    }
}
