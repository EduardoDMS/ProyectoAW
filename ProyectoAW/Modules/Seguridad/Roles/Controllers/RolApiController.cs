using Microsoft.AspNetCore.Mvc;
using ProyectoAW.Infraestructure.RP;
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
            try
            {
                var roles = await _service.ObtenerTodosRolesAsync();
                return Ok(new Respuesta
                {
                    Exito = true,
                    Mensaje = "Se obtuvieron todos los Roles disponibles",
                    Datos = roles
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
                    Mensaje = "Se presento un error interno del servidor, comuniquese con DMS o intentelo nuevamente",
                });
            }
        }

        [Authorize(Policy = "Roles.Ver")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var rol = await _service.ObtenerRolPorIdAsync(id);
                if (rol == null)
                    return NotFound(new Respuesta
                    {
                        Exito= false,
                        Mensaje = "No se obtuvo el rol"
                    });

                return Ok(new Respuesta
                {
                    Exito = true,
                    Mensaje = "Se obtuvo el rol",
                    Datos = rol
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
                    Mensaje = "Error interno del servidor comuniquese con DMS o intentelo nuevamente"
                });
            }

        }

        [Authorize(Policy = "Roles.Crear")]
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearRolDto request)
        {
            try
            {
                var rol = await _service.CrearRolAsync(request);
                return CreatedAtAction(
                                nameof(ObtenerPorId),
                                new { id = rol.IdRol },
                                new Respuesta
                                {
                                    Exito = true,
                                    Mensaje = "exito al crear el nuevo rol",
                                    Datos = rol
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
                    Mensaje = "No se pudo Crear el rol por un problema interno del servidor"
                });
            }
        }

        [Authorize(Policy = "Roles.Actualizar")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarRolDto request)
        {
            try
            {
                var actualizado = await _service.ActualizarRolAsync(id, request);
                if (!actualizado)
                    return NotFound(new Respuesta
                    {
                        Exito = false,
                        Mensaje = "No se pudo actualizar"
                    });

                return Ok(new Respuesta
                {
                    Exito = true,
                    Mensaje = "Se logro actualizar el producto",
                    Datos = actualizado
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
                    Mensaje = "Error al intentar actualizar el producto , verificar el servidor"
                });
            }
        }
    }
}
