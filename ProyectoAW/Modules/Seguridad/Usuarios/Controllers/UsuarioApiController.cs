using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
//using ProyectoAW.Modules.Seguridad.Autenticacion;
using ProyectoAW.Modules.Seguridad.Usuarios.DTOs.Requests;
using ProyectoAW.Modules.Seguridad.Usuarios.Services;

namespace ProyectoAW.Modules.Seguridad.Usuarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Policy = SeguridadPolicies.Admin)]
    public class UsuarioApiController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioApiController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var usuarios = await _service.ListarUsuariosAsync();

            return Ok(usuarios);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var usuario = await _service.ObtenerUsuarioPorIdAsync(id);

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] CrearUsuarioDto request)
        {
            var usuario = await _service.CrearUsuarioAsync(request);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = usuario!.IdUsuario }, usuario);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id , [FromBody] ActualizarUsuarioDto request)
        {
            var actualizado = await _service.ActualizarUsuarioAsync(id, request);
            if (!actualizado)
                return NotFound();

            return NoContent();
        }


        [HttpPatch("{id:int}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] bool activo)
        {
            var actualizado = await _service.CambiarEstadoAsync(id, activo);

            if (!actualizado)
                return NotFound();

            return NoContent();

        }





    }
}
