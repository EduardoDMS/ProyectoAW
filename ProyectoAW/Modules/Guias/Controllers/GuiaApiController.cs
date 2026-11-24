using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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

    [Authorize(Policy = "Guias.Ver")]
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
                Mensaje = "Hubo un error al consultar la bd para obtener sus guias , comuniquese con soporte o vuelva a intentarlo :( ."
            });
        }
    }

    [Authorize(Policy = "Guias.Ver")]
    [HttpGet("tipo-operacion/{idTipoOperacionGuia:int}")]
    public async Task<IActionResult> ListarPorOperacion(int idTipoOperacionGuia)
    {
        try
        {
            var guiasOpera = await _service.ListarGuiasXtipoOperacionAsync(idTipoOperacionGuia);
            return Ok(new Respuesta
            {
                Exito = true,
                Mensaje = "Se listo correctamente las guias por el tipo de operacion",
                Datos = guiasOpera
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
                Mensaje = "Error interno en el servidor , contactese con DMS"
            });
        }
    }




    
    [Authorize(Policy = "Guias.Ver")]
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

    
    
    [Authorize(Policy = "Guias.Crear")]
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
        catch (DbUpdateException)
        {
            return BadRequest(new Respuesta
            {
                Exito = false,
                Mensaje = "No se pudo guardar la guía. Verifica que el cliente, almacén, producto y usuario enviados existan."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Respuesta
            {
                Exito = false,
                Mensaje = "No se pudo crear la guia , intentelo nuevamente o comuniquese con DMS " + ex.Message
            });
        }
    }

    
    
    [Authorize(Policy = "Guias.Actualizar")]
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

    [Authorize(Policy = "Guias.Eliminar")]
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
