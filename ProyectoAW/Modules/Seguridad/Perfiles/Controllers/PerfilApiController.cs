using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Modules.Seguridad.Perfiles.Dtos.Request;
using ProyectoAW.Modules.Seguridad.Perfiles.Services;

namespace ProyectoAW.Modules.Seguridad.Perfiles.Controllers;

[ApiController]
[Route("api/perfiles")]
[Authorize(Policy = "Administrador")]
public class PerfilApiController : ControllerBase
{
    private readonly IPerfilService _service;

    public PerfilApiController(IPerfilService service)
    {
        _service = service;
    }

    [HttpGet("modulos")]
    public async Task<IActionResult> ListarModulos([FromQuery] int idRol)
    {
        try
        {
            return Ok(new Respuesta
            {
                Exito = true,
                Mensaje = "Módulos disponibles obtenidos correctamente.",
                Datos = await _service.ListarModulosAsync(idRol)
            });
        }
        catch (NegocioException ex)
        {
            return BadRequest(new Respuesta { Exito = false, Mensaje = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        try
        {
            return Ok(new Respuesta
            {
                Exito = true,
                Mensaje = "Perfiles obtenidos correctamente.",
                Datos = await _service.ListarAsync()
            });
        }
        catch (Exception)
        {
            return StatusCode(500, new Respuesta
            {
                Exito = false,
                Mensaje = "No se pudieron obtener los perfiles."
            });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        try
        {
            return Ok(new Respuesta
            {
                Exito = true,
                Mensaje = "Perfil obtenido correctamente.",
                Datos = await _service.ObtenerPorIdAsync(id)
            });
        }
        catch (NegocioException ex)
        {
            return NotFound(new Respuesta { Exito = false, Mensaje = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new Respuesta
            {
                Exito = false,
                Mensaje = "No se pudo obtener el perfil."
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearPerfilDto request)
    {
        try
        {
            var perfil = await _service.CrearAsync(request);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = perfil.IdPerfil }, new Respuesta
            {
                Exito = true,
                Mensaje = "Perfil creado correctamente.",
                Datos = perfil
            });
        }
        catch (NegocioException ex)
        {
            return BadRequest(new Respuesta { Exito = false, Mensaje = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new Respuesta
            {
                Exito = false,
                Mensaje = "No se pudo crear el perfil."
            });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarPerfilDto request)
    {
        try
        {
            await _service.ActualizarAsync(id, request);
            return Ok(new Respuesta
            {
                Exito = true,
                Mensaje = "Perfil actualizado correctamente."
            });
        }
        catch (NegocioException ex)
        {
            return BadRequest(new Respuesta { Exito = false, Mensaje = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new Respuesta
            {
                Exito = false,
                Mensaje = "No se pudo actualizar el perfil."
            });
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
                Exito = true,
                Mensaje = "Perfil eliminado correctamente."
            });
        }
        catch (NegocioException ex)
        {
            return BadRequest(new Respuesta { Exito = false, Mensaje = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new Respuesta
            {
                Exito = false,
                Mensaje = "No se pudo eliminar el perfil."
            });
        }
    }
}
