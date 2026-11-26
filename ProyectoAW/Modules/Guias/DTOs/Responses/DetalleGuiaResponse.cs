namespace ProyectoAW.Modules.Guias.DTOs.Responses
{
    public sealed class DetalleGuiaResponse
    {
        public int IdGuiaDetalle { get; set; }
        public int IdProducto { get; set; }
        public string CodProducto { get; set; } = string.Empty;
        public decimal CantidadEsperada { get; set; }

    }
}
