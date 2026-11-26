using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProyectoAW.Modules.Maestros.Almacenes.DTOs.Requests;
using ProyectoAW.Modules.Maestros.Almacenes.Services;
using ProyectoAW.Infraestructure.RP;
using Microsoft.AspNetCore.Authorization;
using ProyectoAW.Modules.Maestros.Almacenes.DTOs.Responses;

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
        [Authorize(Policy = "Almacenes.Ver")]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var almacenes = await _service.ObtenerAlmacenesTodosAsync();
                return Ok(new Respuesta
                {
                    Exito = true,
                    Mensaje = "Almacenes obtenidos correctamente.",
                    Datos = almacenes
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
            catch (Exception)
            {
                return StatusCode(500, new Respuesta
                {
                    Exito = false,
                    Mensaje = "Ocurrió un error inesperado. Por favor, inténtalo de nuevo más tarde."
                });
            
            }
        }

        [Authorize(Policy = "Almacenes.Ver")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {

            var almacen = await _service.ObtenerAlmacenPorIdAsync(id);

                return Ok(new Respuesta
                {
                    Exito = true,
                    Mensaje = "Se encontro el Almacen",
                    Datos = almacen
                });

            }
            catch (NegocioException ex)
            {
                return BadRequest(new Respuesta
                {
                    Exito = false,
                    Mensaje = ex.Message
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new Respuesta
                {
                    Exito = false,
                    Mensaje = "Ocurrio un error inesperado"
                });
            }
        }

        [HttpPost]
        [Authorize(Policy = "Almacenes.Crear")]
        public async Task<IActionResult> Crear([FromBody] CrearAlmacenDto request)
        {
            try 
            {
                var almacen = await _service.CrearAlmacenAsync(request);
                return CreatedAtAction(nameof(ObtenerPorId), new { id = almacen.IdAlmacen }, almacen);
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
                    Mensaje = "Ocurrió un error inesperado. Por favor, inténtalo de nuevo más tarde."
                });
            }
}

        [HttpPut("{id:int}")]
        [Authorize(Policy = "Almacenes.Actualizar")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarAlmacenDto request)
        {
            try
            {
               var actualizarAlmacen =  await _service.ActualizarAsync(id, request);
                return Ok(new Respuesta
                {
                    Exito = true,
                    Mensaje = "Almacén actualizado correctamente.",
                    Datos = actualizarAlmacen
                });
            }
            catch (NegocioException ex)
            {
                return BadRequest(new Respuesta
                {
                    Exito = false,
                    Mensaje = ex.Message
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new Respuesta
                {
                    Exito = false,
                    Mensaje = "No se pudo actualizar el almacén."
                });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Almacenes.Eliminar")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _service.EliminarAsync(id);
                return Ok(new Respuesta
                {
                    Exito = true,
                    Mensaje = "Se elimino el almacen Exitosamente"
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
            catch (Exception e)
            {
                return StatusCode(500, new Respuesta
                {
                    Exito = false,
                    Mensaje = "Error interno: " + e.Message
                });
            }
        }


        #region METODOS NO USADOS

        //[HttpPatch("{id:int}")]
        //public async Task<IActionResult> CambiarEstado(int id, [FromBody] bool activo)
        //{
        //    try
        //    {
        //        var cambiado = await _service.CambiarEstadoAlmacenAsync(id, activo);
        //        if (!cambiado)
        //            throw new NegocioException("No se pudo realizar el cambio de estado , ERROR");

        //        return Ok(new Respuesta
        //        {
        //            Exito = true,
        //            Mensaje = activo ? "Almacen Activado correctamente" : "Almacen Desactivado Correctamente"
        //        });
        //    }
        //    catch (NegocioException EX)
        //    {
        //        return BadRequest(new Respuesta
        //        {
        //            Exito = false,
        //            Mensaje = EX.Message
        //        });
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, new Respuesta
        //        {
        //            Exito = false,
        //            Mensaje = "Ocurrio un error ineperado , debe esperar e intentarlo nuevamente"
        //        });
        //    }

        //}
        #endregion

    }
}
