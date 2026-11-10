using Microsoft.AspNetCore.Mvc;
using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Modules.Guias.DTOs.Requests;
using ProyectoAW.Modules.Guias.Services;

namespace ProyectoAW.Modules.Guias.Controllers;

[ApiController]
[Route("api/guias")]
public class GuiaApiController : ControllerBase
{
    private readonly IGuiaService _service;

    public GuiaApiController(IGuiaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        try
        {
            var guias = await _service.ObtenerTodosAsync();
            return Ok( new Respuesta
            {
                Exito = true,
                Mensaje = "Listado de Guias , correcto.",
                Datos = guias
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
        catch
        {
            return StatusCode(500, new Respuesta
            {
                Exito = false,
                Mensaje = "Hubo un error al consultar la bd para obtener sus guias , comuniquese conn soporte o vuelva a intentarlo :( ."
            });
        }
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        try
        {
            var guia = await _service.ObtenerPorIdAsync(id);
            return Ok( new Respuesta
            {
                Exito = true,
                Mensaje = "Se encontro la guia solicitada",
                Datos = guia
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
                Mensaje = "Algo fallo al querer obtener la guia solicitada"
            });
        }
    }

    
    
    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearGuiaDto request)
    {
        try
        {
            var guia = await _service.CrearAsync(request);
            return CreatedAtAction(
                                nameof(ObtenerPorId),
                                new { id = guia.IdGuia },
                                new Respuesta
                                {
                                    Exito = true,
                                    Mensaje = "Se creo la nueva guia con exito",
                                    Datos = guia
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
                Mensaje = "No se pudo crear la guia , intentelo nuevamente o comuniquese con DMS"
            });
        }
    }

    
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarGuiaDto request)
    {
        try
        {
            var actualizado = await _service.ActualizarAsync(id, request);
            return Ok(new Respuesta
            {
                Exito = true,
                Mensaje = "Se logro actualizar la guia con exito. ",
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
                Mensaje = "No se pudo Actualizar la guia , intentelo nuevamente o contactese con DMS"
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Cancelar(int id)
    {
        try
        {
            var cancelado = await _service.CancelarAsync(id);

            if (!cancelado)
                throw new NegocioException("No se pudo cancelar la guia ");

            return Ok(new Respuesta
            {
                Exito= true,
                Mensaje = "Se cancelo la guia satisfactoriamente",
                Datos = cancelado
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
        catch
        {
            return StatusCode(500, new Respuesta
            {
                Exito = false,
                Mensaje = "No se pudo cancelar la guia verifique si se encuentra en uso o comuniquese con DMS"
            });
        }
    }

}
