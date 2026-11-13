namespace ProyectoAW.Modules.Guias.DTOs.Responses
{
    public record DetalleGuiaResponse(
        int IdGuiaDetalle,
        int IdProducto,
        decimal CantidadEsperada // EL LA ENTIDAD DE DETALLEGUIA ES CANTIDAD NO CANTIDAD ESPERADA POS NOMAS XD
    );
}
