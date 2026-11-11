using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Modules.Maestros.Categorias.DTOs.Requests;
using ProyectoAW.Modules.Maestros.Categorias.Services;

namespace ProyectoAW.Modules.Maestros.Categorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaApiController : ControllerBase
    {
        private readonly ICategoriaService _service;

        public CategoriaApiController(ICategoriaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodasCategorias()
        {
            try
            {
                var categorias = await _service.ListarCategoriasAsync();
                return Ok( new Respuesta
                {
                    Mensaje = "Categorías obtenidas correctamente.",
                    Exito = true,
                    Datos = categorias
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
            catch(Exception)
            {
                return StatusCode(500, new Respuesta
                {
                    Mensaje = "Ocurrió un error al obtener las categorías.",
                    Exito = false
                });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerCategiriaId(int id)
        {
            try
            {
                var categoria = await _service.ObtenerCategoriaPorIdAsync(id);
                return Ok(new Respuesta
                {
                    Mensaje = "Categoría obtenida correctamente.",
                    Exito = true,
                    Datos = categoria
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
                    Mensaje = "Ocurrió un error al obtener la categoría.",
                    Exito = false
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearCategoria([FromBody] CrearCategoriaDto dto)
        {
            try 
            { 
            /// se usa nameof para obtener el nombre del método ObtenerCategiriaId y así poder generar la ruta correcta para el recurso creado
                var categoriaCreada = await _service.CrearCategoriaAsync(dto);
                return CreatedAtAction(
                    nameof(ObtenerCategiriaId),
                    new { id = categoriaCreada.IdCategoria },
                    new Respuesta
                    {
                        Mensaje = "Categoría creada correctamente.",
                        Exito = true,
                        Datos = categoriaCreada
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
                    Mensaje = "Ocurrió un error al crear la categoría.",
                    Exito = false
                });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarCategoria(int id ,[FromBody] ActualizarCategoriaDto request)
        {
            try
            {
                var actualizado = await _service.ActualizarCategoriaAsync(id, request);
                if (!actualizado)
                    throw new NegocioException("No se pudo actualizar la categoría.");

                return Ok(new Respuesta
                {
                    Mensaje = "Categoría actualizada correctamente.",
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
                    Mensaje = "Ocurrió un error al actualizar la categoría.",
                    Exito = false
                });
            }
            
        }

        [HttpPatch("{id:int}/estado")]
        public async Task<IActionResult> CambiarEstadoCategoria(int id, [FromBody] bool activo)
        {
            try
            {
                var actualizado = await _service.CambiarEstadoCategoriaAsync(id, activo);

                if (!actualizado)
                    throw new NegocioException("No se pudo actualizar");

                return Ok(new Respuesta
                {
                    Mensaje = "Estado de la categoría actualizado correctamente.",
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
                    Mensaje = "Ocurrió un error al cambiar el estado de la categoría.",
                    Exito = false
                });
            }
        }
    }
}
