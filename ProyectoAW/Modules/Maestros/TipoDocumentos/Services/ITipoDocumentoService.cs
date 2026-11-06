using ProyectoAW.Modules.Maestros.TipoDocumentos.DTOs.Requests;
using ProyectoAW.Modules.Maestros.TipoDocumentos.DTOs.Response;

namespace ProyectoAW.Modules.Maestros.TipoDocumentos.Services
{
    public interface ITipoDocumentoService
    {
        Task<List<TipoDocumentoDto>> ObtenerTiposDocumentosAsync();
        Task<TipoDocumentoDto> CrearTipoDocumento(CrearTipoDocumentoDto request);
    }
}
