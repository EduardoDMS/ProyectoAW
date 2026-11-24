using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoAW.Modules.Maestros.MotivosGuia.Controllers
{
    [Route("Maestros/MotivoGuia")]
    [Authorize(Policy = "MotivosGuia.Ver")]
    public class MotivoGuiaController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            RouteData.Values["viewModule"] = "Maestros/MotivosGuia";
            return View();
        }
    }
}
