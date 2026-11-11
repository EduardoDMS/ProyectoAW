using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Maestros.UnidadesMedida.Entities;
using ProyectoAW.Modules.Maestros.UnidadesMedida.Services;
using ProyectoAW.Infraestructure.MVC;
using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Modules.Maestros.UnidadesMedida.DTOs.Requests;

namespace ProyectoAW.Modules.Maestros.UnidadesMedida.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UnidadMedidaApiController : ControllerBase
{
    private readonly IUnidadMedidaService _service;

    public UnidadMedidaApiController(IUnidadMedidaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodas()
    {
        try
        {
            var unidadesMedida = await _service.ObtenerTodasAsync();
            return Ok(new Respuesta
            {
                Mensaje = "Unidades de medida obtenidas",
                Exito = true,
                Datos = unidadesMedida
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
                Mensaje = "Ocurrio un error al intentar obtener todas las unidades de medida"
            });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        try
        {
            var producto = await _service.ObtenerPorIdAsync(id);
            return Ok(new Respuesta
            {
                Mensaje = "Se encontro la unidad de medida",
                Exito = true,
                Datos = producto
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
                Mensaje = "Problema en el servidor , intentelos nuevamente "
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearUnidadMedidaDto request)
    {
        try
        {
            var unidadMedida = await _service.CrearAsync(request);
            return CreatedAtAction(
                                    nameof(ObtenerPorId),
                                    new { id = unidadMedida.IdUnidadUnidad },
                                    new Respuesta
                                    {
                                        Mensaje = "Unidad de Medida Creada Correctamente",
                                        Exito = true,
                                        Datos = unidadMedida
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
                Mensaje = "No se pudo crear la unidad de medida",
                Exito = false
            });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarUnidadMedidaDto request)
    {
        try
        {
            var actualizado = await _service.ActualizarAsync(id, request);

            if (!actualizado)
                throw new NegocioException("Unidad de Medida no fue posible actualizar");

            return Ok(new Respuesta
            {
                Mensaje = "Se actualizo con exito la Unidad de Medida",
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
                Exito = false,
                Mensaje = "Hubo un problema intenta mas tarde actualizar tuunidad de medida "
            });
        }

    }

    [HttpPatch("{id:int}/estado")]
    public async Task<IActionResult>CambiarEstado(int id ,[FromBody] bool activo)
    {
        try
        {
            var actualizado = await _service.CambiarEstadoAsync(id, activo);

            if (!actualizado)
                throw new NegocioException("No se pudo cambiar el estado de la unidad de medida solicitada");

            return Ok(new Respuesta
            {
                Exito = true,
                Mensaje = "Se cambio el estado de la unidad de medida solicitada"
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
                Mensaje = "No se logro completar el cambio de estado"
            });
        }
    }

}
