namespace ProyectoAW.Modules.Maestros.Productos.DTOs.Responses
{
    public class ProductoAlmacenDto
    {
        public int IdProducto { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Lote { get; set; }
        public string Serie { get; set; } = string.Empty;
        public string NombreCategoria { get; set; } = string.Empty;
        public string NombreUnidadMedida { get; set; } = string.Empty;
        public int CantidadActual { get; set; }
    }
}
