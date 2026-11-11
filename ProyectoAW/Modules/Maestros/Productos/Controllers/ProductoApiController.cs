using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using ProyectoAW.Modules.Maestros.Productos.DTOs.Requests;
using ProyectoAW.Modules.Maestros.Productos.Services;
using ProyectoAW.Infraestructure.RP;


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
    }
}
