using Microsoft.AspNetCore.Mvc;
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
        var guias = await _service.ObtenerTodosAsync();
        return Ok(guias);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var guia = await _service.ObtenerPorIdAsync(id);

        if (guia == null)
            return NotFound();

        return Ok(guia);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearGuiaDto request)
    {
        var guia = await _service.CrearAsync(request);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = guia.IdGuia }, guia);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarGuiaDto request)
    {
        var actualizado = await _service.ActualizarAsync(id, request);

        if (!actualizado)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Cancelar(int id)
    {
        var cancelado = await _service.CancelarAsync(id);

        if (!cancelado)
            return NotFound();

        return NoContent();
    }
}
