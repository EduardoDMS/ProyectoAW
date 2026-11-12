using Microsoft.AspNetCore.Mvc;
using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Modules.Guias.DTOs.Requests;
using ProyectoAW.Modules.Guias.Services;

namespace ProyectoAW.Modules.Guias.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MotivoGuiaApiController : ControllerBase
{
    private readonly IMotivoGuiaService _service;

    public MotivoGuiaApiController(IMotivoGuiaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] int? idOperacion, [FromQuery] bool soloActivos = false)
    {
        try
        {
            var motivos = await _service.ListarAsync(idOperacion, soloActivos);
            return Ok(new Respuesta
            {
                Mensaje = "Motivos obtenidos exitosamente.",
                Exito = true,
                Datos = motivos
            });
        }
        catch (NegocioException ex)
        {
            return BadRequest(new Respuesta { Mensaje = ex.Message, Exito = false });
        }
        catch (Exception)
        {
            return StatusCode(500, new Respuesta { Mensaje = "Ocurrio un error inesperado.", Exito = false });
        }
    }

    [HttpGet("comportamientos")]
    public async Task<IActionResult> ListarComportamientos()
    {
        try
        {
            var tipos = await _service.ListarComportamientosAsync();
            return Ok(new Respuesta
            {
                Mensaje = "Tipos de comportamiento obtenidos exitosamente.",
                Exito = true,
                Datos = tipos
            });
        }
        catch (NegocioException ex)
        {
            return BadRequest(new Respuesta { Mensaje = ex.Message, Exito = false });
        }
        catch (Exception)
        {
            return StatusCode(500, new Respuesta { Mensaje = "Ocurrio un error inesperado.", Exito = false });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Obtener(int id)
    {
        try
        {
            var motivo = await _service.ObtenerAsync(id);
            return Ok(new Respuesta
            {
                Mensaje = "Motivo encontrado.",
                Exito = true,
                Datos = motivo
            });
        }
        catch (NegocioException ex)
        {
            return BadRequest(new Respuesta { Mensaje = ex.Message, Exito = false });
        }
        catch (Exception)
        {
            return StatusCode(500, new Respuesta { Mensaje = "Ocurrio un error inesperado.", Exito = false });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearMotivoGuiaDto request)
    {
        try
        {
            var motivo = await _service.CrearAsync(request);
            return CreatedAtAction(nameof(Obtener), new { id = motivo.IdMotivoGuia }, new Respuesta
            {
                Mensaje = "Motivo creado exitosamente.",
                Exito = true,
                Datos = motivo
            });
        }
        catch (NegocioException ex)
        {
            return BadRequest(new Respuesta { Mensaje = ex.Message, Exito = false });
        }
        catch (Exception)
        {
            return StatusCode(500, new Respuesta { Mensaje = "Ocurrio un error inesperado.", Exito = false });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarMotivoGuiaDto request)
    {
        try
        {
            var actualizado = await _service.ActualizarAsync(id, request);
            if (!actualizado)
                throw new NegocioException("No fue posible actualizar el motivo.");

            return Ok(new Respuesta
            {
                Mensaje = "Motivo actualizado exitosamente.",
                Exito = true
            });
        }
        catch (NegocioException ex)
        {
            return BadRequest(new Respuesta { Mensaje = ex.Message, Exito = false });
        }
        catch (Exception)
        {
            return StatusCode(500, new Respuesta { Mensaje = "Ocurrio un error inesperado.", Exito = false });
        }
    }

    [HttpPatch("{id:int}/estado")]
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] bool activo)
    {
        try
        {
            var actualizado = await _service.CambiarEstadoAsync(id, activo);
            if (!actualizado)
                throw new NegocioException("No fue posible actualizar el estado del motivo.");

            return Ok(new Respuesta
            {
                Mensaje = "Estado del motivo actualizado exitosamente.",
                Exito = true
            });
        }
        catch (NegocioException ex)
        {
            return BadRequest(new Respuesta { Mensaje = ex.Message, Exito = false });
        }
        catch (Exception)
        {
            return StatusCode(500, new Respuesta { Mensaje = "Ocurrio un error inesperado.", Exito = false });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            await _service.EliminarAsync(id);
            return Ok(new Respuesta
            {
                Mensaje = "Motivo eliminado exitosamente.",
                Exito = true
            });
        }
        catch (NegocioException ex)
        {
            return BadRequest(new Respuesta { Mensaje = ex.Message, Exito = false });
        }
        catch (Exception)
        {
            return StatusCode(500, new Respuesta { Mensaje = "Ocurrio un error inesperado.", Exito = false });
        }
    }
}
