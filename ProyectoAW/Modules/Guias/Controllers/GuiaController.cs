using Microsoft.AspNetCore.Mvc;

namespace ProyectoAW.Modules.Guias.Controllers
{
    [Route("Guias")]
    public class GuiaController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            RouteData.Values["viewModule"] = "Guias";
            return View();
        }
    }
}
