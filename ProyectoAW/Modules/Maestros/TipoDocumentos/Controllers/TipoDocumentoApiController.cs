using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAW.Modules.Maestros.TipoDocumentos.DTOs.Requests;
using ProyectoAW.Modules.Maestros.TipoDocumentos.Services;
using ProyectoAW.Infraestructure.RP;

namespace ProyectoAW.Modules.Maestros.TipoDocumentos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoDocumentoApiController : ControllerBase
    {
        private readonly ITipoDocumentoService _service;

        public TipoDocumentoApiController(ITipoDocumentoService service)
        {
            _service = service;
        }

        [Authorize(Policy = "TipoDocumentos.Ver")]
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            try
            {
                var TipoDocumentos = await _service.ObtenerTiposDocumentosAsync();
                return Ok(new Respuesta
                {
                    Mensaje = "Tipo de Documentos obtenidos exitosamente",
                    Exito = true,
                    Datos = TipoDocumentos
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
            catch(Exception)
            {
                return StatusCode(500, new Respuesta
                {
                    Exito = false,
                    Mensaje = "Error interno en el servidor utilize el ex.message para averiguar que sucedio"
                });
            }
        }

    }
}
