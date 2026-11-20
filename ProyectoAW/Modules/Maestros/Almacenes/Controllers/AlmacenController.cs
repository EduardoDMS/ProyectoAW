using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoAW.Modules.Maestros.Almacenes.Controllers
{
    [Route("Maestros/Almacen")]
    [Authorize(Policy = "Almacenes.Ver")]
    public class AlmacenController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            RouteData.Values["viewModule"] = "Maestros/Almacenes";
            return View();
        }
    }
}
