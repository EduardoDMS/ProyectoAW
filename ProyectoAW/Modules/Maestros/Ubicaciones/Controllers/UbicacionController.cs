using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoAW.Modules.Maestros.Ubicaciones.Controllers
{
    [Route("Maestros/Ubicacion")]
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
