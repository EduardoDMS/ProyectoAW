using System.ComponentModel.DataAnnotations;

namespace ProyectoAW.Modules.Stocks.Dtos.Request
{
    public class RegistrarLecturaGuiaRequest
    {
        // se usa init para darle un valor inicial y despues de eso esta propiedad sea inmutable al querer acceder a ella
        [Range(1, int.MaxValue)]
        public int IdProducto { get; init; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; init; }

        [Required]
        [StringLength(100)]
        public string CodigoLeido { get; init; } = string.Empty;

        [StringLength(100)]
        public string? Lote { get; init; }

        [StringLength(100)]
        public string? Serie { get; init; }

        [StringLength(300)]
        public string? Observacion { get; init; }
        
        public int? IdUbicacionOrigen { get; init; }
        
        public int? IdUbicacionDestino { get; init; }
    }
}
