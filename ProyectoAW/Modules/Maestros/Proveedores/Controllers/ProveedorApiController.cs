using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using ProyectoAW.Modules.Maestros.Proveedores.DTOs.Requests;
using ProyectoAW.Modules.Maestros.Proveedores.Services;
using ProyectoAW.Infraestructure.RP;

namespace ProyectoAW.Modules.Maestros.Proveedores.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[EnableCors("PwaRead")]
    public class ProveedorApiController : ControllerBase
    {
        private readonly IProveedorService _service;

        public ProveedorApiController(IProveedorService service)
        {
            _service=service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var proveedores = await _service.ObtenerTodosAsync();
                return Ok(new Respuesta
                {
                    Exito= true,
                    Mensaje = "Se obtuvieron los datos de proveedores",
                    Datos = proveedores
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
                    Mensaje = "Ocurrio un problema externo , contactate con DMS",
                    Exito = false
                });
            }
        
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var proveedor = await _service.ObtenerPorIdAsync(id);
                return Ok(new Respuesta
                {
                    Mensaje = "Proveedor encontrado.",
                    Exito = true,
                    Datos = proveedor
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
                return StatusCode(500, new Respuesta
                {
                    Mensaje = "Ocurrió un error inesperado.",
                    Exito = false
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearProveedorDto request)
        {
            try
            {

                var proveedor = await _service.CrearAsync(request);
                return CreatedAtAction(
                                nameof(ObtenerPorId),
                                new { id = proveedor.IdProveedor },
                                new Respuesta
                                {
                                    Mensaje = "Proveedor creado exitosamente.",
                                    Exito = true,
                                    Datos = proveedor
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
                return StatusCode(500, new Respuesta
                {
                    Mensaje = "Ocurrió un error inesperado.",
                    Exito = false
                });
            }

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarProveedorDto request)
        {
            try
            {
                var actualizado = await _service.ActualizarAsync(id, request);
                if (!actualizado)
                    throw new NegocioException("No se pudo realizar la actualisacion correspondiente.");

                return Ok(new Respuesta
                {
                    Mensaje = "Proveedor actualizado exitosamente.",
                    Exito = true
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
                return StatusCode(500, new Respuesta
                {
                    Mensaje = "Ocurrió un error inesperado.",
                    Exito = false
                });
            }
        }

        [HttpPatch("{id:int}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] bool activo)
        {
            try
            {
                var actualizado = await _service.CambiarEstadoAsync(id, activo);

                if (!actualizado)
                    throw new NegocioException("Proveedor no encontrado.");

                return Ok(new Respuesta
                {
                    Mensaje = "Estado del proveedor actualizado exitosamente.",
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
                return StatusCode(500, new Respuesta
                {
                    Mensaje = "Ocurrió un error inesperado.",
                    Exito = false
                });
            }
        }
    }
}
