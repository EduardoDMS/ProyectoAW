using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using ProyectoAW.Modules.Maestros.Ubicaciones.DTOs.Requests;
using ProyectoAW.Modules.Maestros.Ubicaciones.Services;
using ProyectoAW.Infraestructure.RP;

namespace ProyectoAW.Modules.Maestros.Ubicaciones.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
   // [EnableCors("PwaRead")]
    public class UbicacionApiController : ControllerBase
    {
        private readonly IUbicacionService _service;

        public UbicacionApiController(IUbicacionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos([FromQuery] int? idAlmacen, [FromQuery] bool soloActivas = false)
        {
            try
            {
                var ubicaciones = await _service.ObtenerUbicacionesAsync(idAlmacen, soloActivas);
                return Ok(new Respuesta
                {
                    Mensaje = "Ubicaciones obtenidas exitosamente.",
                    Exito = true,
                    Datos = ubicaciones
                });
            }
            catch(NegocioException ex)
            {
                return BadRequest(new Respuesta
                {
                    Mensaje = ex.Message,
                    Exito = false
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta
                {
                    Mensaje = "Ocurrió un error inesperado.",
                    Exito = false
                });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var ubicacion = await _service.ObtenerUbicacionPorIdAsync(id);
                return Ok(new Respuesta
                {
                    Mensaje = "Ubicación obtenida exitosamente.",
                    Exito = true,
                    Datos = ubicacion
                });
            }
            catch (NegocioException ex)
            {
                return BadRequest(new Respuesta
                {
                    Mensaje = ex.Message,
                    Exito = false
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta
                {
                    Mensaje = "Ocurrió un error inesperado.",
                    Exito = false
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearUbicacionDto request)
        {
            try
            {
                var ubicacion = await _service.CrearUbicacionAsync(request);
                return CreatedAtAction(
                                    nameof(ObtenerPorId),
                                    new { id = ubicacion.IdUbicacion },
                                    new Respuesta
                                    {
                                        Mensaje = "Ubicación creada exitosamente.",
                                        Exito = true,
                                        Datos = ubicacion
                                    });
            }
            catch (NegocioException ex)
            {
                return BadRequest(new Respuesta
                {
                    Mensaje = ex.Message,
                    Exito = false
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta
                {
                    Mensaje = "Ocurrió un error inesperado.",
                    Exito = false
                });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarUbicacionDto request)
        {
            try
            {
                var resultado = await _service.ActualizarAsync(id, request);
                if (!resultado)
                    throw new NegocioException($"No se pudo actualizar la ubicación con ID {id}");

                return Ok(new Respuesta
                {
                    Mensaje = "Ubicación actualizada exitosamente.",
                    Exito = true
                });
            }
            catch (NegocioException ex)
            {
                return BadRequest(new Respuesta
                {
                    Mensaje = ex.Message,
                    Exito = false
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta
                {
                    Mensaje = "Ocurrió un error inesperado.",
                    Exito = false
                });
            }
        }

        [HttpPatch("{id:int}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromQuery] bool activo)
        {
            try
            {
                var resultado = await _service.CambiarEstadoAsync(id, activo);
                if (!resultado)
                    throw new NegocioException($"No se pudo cambiar el estado de la ubicación con ID {id}");

                return Ok(new Respuesta
                {
                    Mensaje = "Estado de la ubicación cambiado exitosamente.",
                    Exito = true
                });
            }
            catch (NegocioException ex)
            {
                return BadRequest(new Respuesta
                {
                    Mensaje = ex.Message,
                    Exito = false
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta
                {
                    Mensaje = "Ocurrió un error inesperado.",
                    Exito = false
                });
            }
            
        }

    }
}
