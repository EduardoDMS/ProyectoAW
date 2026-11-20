using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoAW.Modules.Maestros.Clientes.Controllers
{
    [Route("Maestros/Cliente")]
    [Authorize(Policy = "Clientes.Ver")]
    public class ClienteController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            RouteData.Values["viewModule"] = "Maestros/Clientes";
            return View();
        }
    }
}
