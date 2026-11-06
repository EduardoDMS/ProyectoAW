using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProyectoAW.Modules.Maestros.Almacenes.DTOs.Requests;
using ProyectoAW.Modules.Maestros.Almacenes.Services;

namespace ProyectoAW.Modules.Maestros.Almacenes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("PwaRead")]
    public class AlmacenApiController : ControllerBase
    {
        private readonly IAlmacenService _service;
        
        public AlmacenApiController(IAlmacenService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var almacenes = await _service.ObtenerAlmacenesTodosAsync();
            return Ok(almacenes);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var almacen = await _service.ObtenerAlmacenPorIdAsync(id);

            if (almacen == null)
                return NotFound();
            
            return Ok(almacen);
        }


        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearAlmacenDto request)
        {
            var almacen = await _service.CrearAlmacenAsync(request);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = almacen.IdAlmacen }, almacen);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] bool activo)
        {
            var cambiado = await _service.CambiarEstadoAlmacenAsync(id, activo);
            
            if (!cambiado)
                return NotFound();
            
            return NoContent();
        }



    }
}
