using Microsoft.AspNetCore.Mvc;

namespace ProyectoAW.Modules.Maestros.Categorias.Controllers
{
    [Route("Maestros/Categoria")]
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
