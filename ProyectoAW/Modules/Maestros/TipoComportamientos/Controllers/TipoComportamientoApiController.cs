using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Modules.Maestros.TipoComportamientos.Services;

namespace ProyectoAW.Modules.Maestros.TipoComportamientos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoComportamientoApiController : ControllerBase
    {
        private readonly ITipoComportamientoService _service;

        public TipoComportamientoApiController( ITipoComportamientoService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ListatTodosTipos()
        {
            try
            {
                var lista = await _service.ListarTodo();

                return Ok(new Respuesta
                {
                    Exito = true,
                    Mensaje = " Tipos de comportamiento obtenidos con exito",
                    Datos = lista
                });
            }
            catch(NegocioException ex)
            {
                return BadRequest(new Respuesta
                {
                    Exito = false,
                    Mensaje = ex.Message
                });
            }
            catch(Exception)
            {
                return StatusCode(500, new Respuesta
                {
                    Exito = false,
                    Mensaje = "No se pudo obtener los tipos de comportamiento"
                });
            }
        }


    }
}
