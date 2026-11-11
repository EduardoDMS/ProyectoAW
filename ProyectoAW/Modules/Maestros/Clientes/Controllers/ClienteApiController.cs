using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using ProyectoAW.Modules.Maestros.Clientes.DTOs.Requests;
using ProyectoAW.Modules.Maestros.Clientes.Services;
using ProyectoAW.Infraestructure.RP;
using System.Reflection.Metadata.Ecma335;

namespace ProyectoAW.Modules.Maestros.Clientes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[EnableCors("PwaRead")]
    public class ClienteApiController : ControllerBase
    {
        private readonly IClienteService _service;

        public ClienteApiController(IClienteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var clientes = await _service.ObtenerTodosAsync();
                return Ok(new Respuesta
                {
                    Mensaje = "Clientes obtenidos correctamente.",
                    Exito = true,
                    Datos = clientes
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var cliente = await _service.ObtenerPorIdAsync(id);

                if (cliente == null)
                    throw new NegocioException("Cliente no encontrado.");

                return Ok(new Respuesta
                {
                    Mensaje = "Cliente encontrado.",
                    Exito = true,
                    Datos = cliente
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
                    Mensaje = "Ocurrió un error al obtener el cliente.",
                    Exito = false
                });
            }
                    }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearClienteDto request)
        {
            try
            {
                var cliente = await _service.CrearAsync(request);
                
                return CreatedAtAction(
                            nameof(ObtenerPorId),
                            new { id = cliente.IdCliente },
                            new Respuesta
                            {
                                Mensaje = "Cliente creado correctamente.",
                                Exito = true,
                                Datos = cliente
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
                    Mensaje = "Ocurrió un error al crear el cliente.",
                    Exito = false
                });
            }

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarClienteDto request)
        {
            try
            {
                var actualizado = await _service.ActualizarAsync(id, request);
                if (!actualizado)
                    throw new NegocioException("Cliente no fue posible actualizarlo.");

                return Ok(new Respuesta
                {
                    Mensaje = "Cliente actualizado correctamente.",
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
                    Mensaje = "Ocurrió un error al actualizar el cliente.",
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
                    throw new NegocioException("Cliente no encontrado.");

                return Ok(new Respuesta
                {
                    Mensaje = "Estado del cliente actualizado correctamente.",
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
            catch (Exception ex)
            {
                return StatusCode(500, new Respuesta
                {
                    Mensaje = ex.Message,
                    Exito = false
                });
            }
        }
    }
}
