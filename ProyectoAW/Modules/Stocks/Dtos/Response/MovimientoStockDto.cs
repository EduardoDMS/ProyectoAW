namespace ProyectoAW.Modules.Stocks.Dtos.Response
{
    public sealed record MovimientoStockDto
        (
            int IdMovimientoStock,
            int IdProducto,
            int IdAlmacen,
            int IdTipoOperacion,
            int Cantidad,
            string FchMovimiento,
            int IdGuia,
            string Observacion,
            int CantidadDiferenciada,
            int? IdUbicacionOrigen,
            int? IdUbicacionDestino,
            //no estan en MovimientoStock se obtienen ingresando a la entidad 
            string NumeroGuia,
            string MotivoGuia,
            string Operacion,
            string CodigoProducto,
            string Producto,
            string Origen,
            string Destino,
            decimal CantidadEsperada,
            int IdUsuarioOperador,
            string Operador,
            string UbicacionOrigen,
            string UbicacionDestino
        );
}
