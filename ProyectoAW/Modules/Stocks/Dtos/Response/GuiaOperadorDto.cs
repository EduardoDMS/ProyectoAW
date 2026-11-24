using ProyectoAW.Modules.Guias.DTOs.Responses;

namespace ProyectoAW.Modules.Stocks.Dtos.Response
{
    public class GuiaOperadorDto
    {
        public int IdGuia { get; set; }
        public string NumeroGuia { get; set; } = string.Empty;
        public int IdTipoOperacionGuia { get; set; }
        public int? IdMotivoGuia { get; set; }
        public int? IdTipoComportamiento { get; set; }
        public string? NombreMotivo { get; set; }
        public bool RequiereProveedor { get; set; }
        public bool RequiereCliente { get; set; }
        public bool RequiereAlmacenOrigen { get; set; }
        public bool RequiereAlmacenDestino { get; set; }
        public string? TipoTercero { get; set; }
        public string? NombreTercero { get; set; }
        public string? NombreAlmacenOrigen { get; set; }
        public string? NombreAlmacenDestino { get; set; }
        public int? IdGuiaRelacionada { get; set; }
        public string? NumeroGuiaRelacionada { get; set; }
        public int IdEstadoGuia { get; set; }
        public int? IdProveedor { get; set; }
        public int? IdCliente { get; set; }
        public int? IdAlmacenOrigen { get; set; }
        public int? IdAlmacenDestino { get; set; }
        public string FechaProgramada { get; set; } = string.Empty;
        public string FechaRegistro { get; set; } = string.Empty;
        public string? FechaActualizada { get; set; }
        public string? TipoDocumentoReferencia { get; set; }
        public string? NumeroDocumentoReferencia { get; set; }
        public string? Observacion { get; set; }
        public IReadOnlyList<DetalleGuiaResponse> Detalles { get; set; } = Array.Empty<DetalleGuiaResponse>();
    }
}
