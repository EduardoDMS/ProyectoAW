using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoAW.Modules.Maestros.Categorias.Controllers
{
    [Route("Maestros/Categoria")]
    [Authorize(Policy = "Categorias.Ver")]
    public class CategoriaController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            RouteData.Values["viewModule"] = "Maestros/Categorias";
            return View();
        }
    }
}
