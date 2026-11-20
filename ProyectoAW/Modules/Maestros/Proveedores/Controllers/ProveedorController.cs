using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoAW.Modules.Maestros.Proveedores.Controllers
{
    [Route("Maestros/Proveedor")]
    [Authorize(Policy = "Proveedores.Ver")]
    public class ProveedorController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            RouteData.Values["viewModule"] = "Maestros/Proveedores";
            return View();
        }
    }
}
