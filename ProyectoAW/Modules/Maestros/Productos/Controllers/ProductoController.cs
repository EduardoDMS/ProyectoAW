using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoAW.Modules.Maestros.Productos.Controllers
{
    [Route("Maestros/Producto")]
    [Authorize(Policy = "Productos.Ver")]
    public class ProductoController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            RouteData.Values["viewModule"] = "Maestros/Productos";
            return View();
        }
    }
}
