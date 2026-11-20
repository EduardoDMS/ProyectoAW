using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using ProyectoAW.Modules.Maestros.Productos.DTOs.Requests;
using ProyectoAW.Modules.Maestros.Productos.Services;
using ProyectoAW.Infraestructure.RP;


using Microsoft.AspNetCore.Authorization;

namespace ProyectoAW.Modules.Maestros.Productos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[EnableCors("PwaRead")]
    public class ProductoApiController : ControllerBase
    {
        private readonly IProductoService _service;

        public ProductoApiController(IProductoService service)
        {
            _service = service;
        }

        [Authorize(Policy = "Productos.Ver")]
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var productos = await _service.ObtenerTodosAsync();
                return Ok(new Respuesta
                {
                    Mensaje = "Productos obtenidos exitosamente.",
                    Exito = true,
                    Datos = productos
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

        [Authorize(Policy = "Productos.Ver")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var producto = await _service.ObtenerPorIdAsync(id);
                return Ok( new Respuesta
                {
                    Mensaje = "Producto encontrado.",
                    Exito = true,
                    Datos = producto
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

        [Authorize(Policy = "Productos.Crear")]
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearProductoDto request)
        {
            try
            {
                var producto = await _service.CrearAsync(request);
                return CreatedAtAction(
                                    nameof(ObtenerPorId),
                                    new { id = producto.IdProducto },
                                    new Respuesta
                                    {
                                        Mensaje = "Producto creado exitosamente.",
                                        Exito = true,
                                        Datos = producto
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

        [Authorize(Policy = "Productos.Actualizar")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarProductoDto request)
        {
            try
            {
                var actualizado = await _service.ActualizarAsync(id, request);
                if (!actualizado)
                    throw new NegocioException("Producto no fue posible actualizar");

                return Ok(new Respuesta
                {
                    Mensaje = "Producto actualizado exitosamente.",
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

        [Authorize(Policy = "Productos.Actualizar")]
        [HttpPatch("{id:int}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] bool activo) 
        {
            try
            {
                var actualizado = await _service.CambiarEstadoAsync(id, activo);

                if (!actualizado)
                    throw new NegocioException("Producto no fue posible actualizar");

                return Ok(new Respuesta
                {
                    Mensaje = "Estado del producto actualizado exitosamente.",
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

        [Authorize(Policy = "Productos.Eliminar")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarProducto(int idProducto)
        {
            try
            {
                await _service.EliminarProductoAsync(idProducto);
                return Ok(new Respuesta
                {
                    Exito = true,
                    Mensaje = "Se logro eliminar el producto seleccionado"
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
                    Mensaje = "Error interno del servido contactese de inmediato con DMS"
                    // prueba ex.Message para rasterear el error
                });
            }
        }
    }
}
