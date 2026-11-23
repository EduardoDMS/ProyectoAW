namespace ProyectoAW.Modules.Stocks.Dtos.Response
{
    public sealed class LecturaGuiaDto
    {
        public long IdLecturaGuia { get; set; }
        public int IdGuia { get; set; }
        public int IdProducto { get; set; }
        public string CodigoProducto { get; set; } = string.Empty;
        public string DescripcionProducto { get; set; } = string.Empty;
        public string CodigoLeido { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public string? Lote { get; set; }
        public string? Serie { get; set; }
        public string? Observacion { get; set; }
        public int? IdUbicacionOrigen { get; set; }
        public string? UbicacionOrigen { get; set; }
        public int? IdUbicacionDestino { get; set; }
        public string? UbicacionDestino { get; set; }
        public string FechaRegistroUtc { get; set; } = string.Empty;
    }
}
