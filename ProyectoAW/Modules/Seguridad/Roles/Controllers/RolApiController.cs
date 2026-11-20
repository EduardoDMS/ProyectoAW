using Microsoft.AspNetCore.Mvc;
using ProyectoAW.Modules.Seguridad.Roles.DTOs.Requests;
using ProyectoAW.Modules.Seguridad.Roles.Services;

using Microsoft.AspNetCore.Authorization;

namespace ProyectoAW.Modules.Seguridad.Roles.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolApiController : ControllerBase
    {
        private readonly IRolService _service;

        public RolApiController(IRolService service)
        {
            _service = service;
        }


        [Authorize(Policy = "Roles.Ver")]
        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            var roles = await _service.ObtenerTodosRolesAsync();
            return Ok(roles);
        }

        [Authorize(Policy = "Roles.Ver")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var rol = await _service.ObtenerRolPorIdAsync(id);

            if (rol == null)
                return NotFound();

            return Ok(rol);
        }

        [Authorize(Policy = "Roles.Crear")]
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearRolDto request)
        {
            var rol = await _service.CrearRolAsync(request);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = rol.IdRol }, rol);
        }

        [Authorize(Policy = "Roles.Actualizar")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarRolDto request)
        {
            var actualizado = await _service.ActualizarRolAsync(id, request);
            if (!actualizado)
                return NotFound();

            return NoContent();
        }




    }
}
