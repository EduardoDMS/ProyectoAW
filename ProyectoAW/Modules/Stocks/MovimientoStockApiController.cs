using Microsoft.AspNetCore.Mvc;
using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Modules.Stocks.Dtos.Request;
using ProyectoAW.Modules.Stocks.Services;

namespace ProyectoAW.Modules.Stocks.Controllers;

[ApiController]
[Route("api/movimientos-stock")]
public class MovimientoStockApiController : ControllerBase
{
    private readonly IMovimientoStockService _service;

    public MovimientoStockApiController(IMovimientoStockService service)
    {
        _service = service;
    }

    // GET: api/movimientos-stock
    [HttpGet]
    public async Task<IActionResult> Listar(
        [FromQuery] int? idGuia,
        [FromQuery] int? idAlmacen,
        [FromQuery] int? idOperacion)
    {
        try
        {
            var movimientos = await _service.ListarAsync(
                idGuia, idAlmacen, idOperacion);

            return Ok(new Respuesta
            {
                Exito = true,
                Mensaje = "Listado de movimientos de stock obtenido correctamente.",
                Datos = movimientos
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
        catch
        {
            return StatusCode(500, new Respuesta
            {
                Exito = false,
                Mensaje = "No se pudieron consultar los movimientos de stock."
            });
        }
    }

    [HttpGet("guias-operador")]
    public async Task<IActionResult> ListarGuiasOperador(
        [FromQuery] int? idEstadoGuia)
    {
        try
        {
            var guias = await _service.ListarGuiasOperadorAsync(idEstadoGuia);

            return Ok(new Respuesta
            {
                Exito = true,
                Mensaje = "Listado de guías del operador obtenido correctamente.",
                Datos = guias
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
        catch
        {
            return StatusCode(500, new Respuesta
            {
                Exito = false,
                Mensaje = "No se pudieron consultar las guías del operador."
            });
        }
    }

    // GET: api/movimientos-stock/guias/15
    [HttpGet("guias/{idGuia:int}")]
    public async Task<IActionResult> ObtenerMiGuia(int idGuia)
    {
        try
        {
            var guia = await _service.ObtenerMiGuiaAsync(idGuia);

            return Ok(new Respuesta
            {
                Exito = true,
                Mensaje = "Guía obtenida correctamente.",
                Datos = guia
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
        catch
        {
            return StatusCode(500, new Respuesta
            {
                Exito = false,
                Mensaje = "No se pudo obtener la guía solicitada."
            });
        }
    }

    // POST: api/movimientos-stock/guias/15/tomar
    [HttpPost("guias/{idGuia:int}/tomar")]
    public async Task<IActionResult> TomarGuia(int idGuia)
    {
        try
        {
            await _service.TomarGuiaAsync(idGuia);

            return Ok(new Respuesta
            {
                Exito = true,
                Mensaje = "La guía fue tomada correctamente.",
                Datos = true
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
        catch
        {
            return StatusCode(500, new Respuesta
            {
                Exito = false,
                Mensaje = "No se pudo tomar la guía. Inténtelo nuevamente."
            });
        }
    }

    // POST: api/movimientos-stock/guias/15/lecturas
    [HttpPost("guias/{idGuia:int}/lecturas")]
    public async Task<IActionResult> RegistrarLectura(
        int idGuia,
        [FromBody] RegistrarLecturaGuiaRequest request)
    {
        try
        {
            var lectura = await _service.RegistrarLecturaAsync(idGuia, request);

            return Ok(new Respuesta
            {
                Exito = true,
                Mensaje = "Lectura registrada correctamente.",
                Datos = lectura
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
        catch
        {
            return StatusCode(500, new Respuesta
            {
                Exito = false,
                Mensaje = "No se pudo registrar la lectura."
            });
        }
    }

    // DELETE: api/movimientos-stock/guias/15/lecturas/100
    [HttpDelete("guias/{idGuia:int}/lecturas/{idLecturaGuia:long}")]
    public async Task<IActionResult> EliminarLectura(
        int idGuia,
        long idLecturaGuia)
    {
        try
        {
            var eliminada = await _service.EliminarLecturaAsync(
                idGuia, idLecturaGuia);

            if (!eliminada)
            {
                return NotFound(new Respuesta
                {
                    Exito = false,
                    Mensaje = "No se encontró la lectura solicitada."
                });
            }

            return Ok(new Respuesta
            {
                Exito = true,
                Mensaje = "Lectura eliminada correctamente.",
                Datos = eliminada
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
        catch
        {
            return StatusCode(500, new Respuesta
            {
                Exito = false,
                Mensaje = "No se pudo eliminar la lectura."
            });
        }
    }

    // POST: api/movimientos-stock/guias/15/finalizar
    [HttpPost("guias/{idGuia:int}/finalizar")]
    public async Task<IActionResult> FinalizarLectura(int idGuia)
    {
        try
        {
            await _service.FinalizarLecturaAsync(idGuia);

            return Ok(new Respuesta
            {
                Exito = true,
                Mensaje = "La lectura de la guía finalizó correctamente.",
                Datos = true
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
        catch
        {
            return StatusCode(500, new Respuesta
            {
                Exito = false,
                Mensaje = "No se pudo finalizar la lectura de la guía."
            });
        }
    }
}
