namespace ProyectoAW.Modules.Stocks.Dtos.Response
{
    public class MovimientoStockDto
    {
        public int IdMovimientoStock { get; set; }
        public int IdProducto { get; set; }
        public int IdAlmacen { get; set; }
        public int IdTipoOperacion { get; set; }
        public int Cantidad { get; set; }
        public string FchMovimiento { get; set; } = string.Empty;
        public int IdGuia { get; set; }
        public string Observacion { get; set; } = string.Empty;
        public int CantidadDiferenciada { get; set; }
        public int? IdUbicacionOrigen { get; set; }
        public int? IdUbicacionDestino { get; set; }

        // No estan en MovimientoStock; se obtienen desde las entidades relacionadas.
        public string NumeroGuia { get; set; } = string.Empty;
        public string MotivoGuia { get; set; } = string.Empty;
        public string Operacion { get; set; } = string.Empty;
        public string CodigoProducto { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;
        public string Origen { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public decimal CantidadEsperada { get; set; }
        public int IdUsuarioOperador { get; set; }
        public string Operador { get; set; } = string.Empty;
        public string UbicacionOrigen { get; set; } = string.Empty;
        public string UbicacionDestino { get; set; } = string.Empty;
    }
}
