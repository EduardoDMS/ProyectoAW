namespace ProyectoAW.Modules.Guias.DTOs.Responses
{
    public sealed class DetalleGuiaResponse
    {
        public int IdGuiaDetalle { get; set; }
        public int IdProducto { get; set; }
        public decimal CantidadEsperada { get; set; }
    }
}
