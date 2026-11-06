using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAW.Modules.Maestros.TipoDocumentos.DTOs.Requests;
using ProyectoAW.Modules.Maestros.TipoDocumentos.Services;

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
    }
}
