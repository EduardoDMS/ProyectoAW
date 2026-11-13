// Archivo: Factories/LecturaGuiaFactory.cs
using ProyectoAW.Modules.Stocks.Dtos.Request;
using ProyectoAW.Modules.Stocks.Entities;

namespace ProyectoAW.Modules.Stocks.Factories
{
    public static class LecturaGuiaFactory
    {
        public static LecturaGuia Crear(int idGuia, int idOperador, RegistrarLecturaGuiaRequest request) => new()
        {
            IdGuia = idGuia,
            IdProducto = request.IdProducto,
            IdUsuarioOperador = idOperador,
            Cantidad = request.Cantidad,
            CodigoLeido = request.CodigoLeido.Trim(),
            Lote = Normalizar(request.Lote),
            Serie = Normalizar(request.Serie),
            Observacion = Normalizar(request.Observacion),
            IdUbicacionOrigen = request.IdUbicacionOrigen is > 0 ? request.IdUbicacionOrigen : null,
            IdUbicacionDestino = request.IdUbicacionDestino is > 0 ? request.IdUbicacionDestino : null,
            FechaRegistroUtc = DateTime.UtcNow
        };

        private static string? Normalizar(string? valor) =>
            string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }
}