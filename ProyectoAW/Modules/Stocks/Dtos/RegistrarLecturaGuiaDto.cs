using System.ComponentModel.DataAnnotations;

namespace ProyectoAW.Modules.Stocks.DTOs;

public class RegistrarLecturaGuiaDto
{
    [Range(1, int.MaxValue)]
    public int IdProducto { get; set; }

    [Range(1, int.MaxValue)]
    public int IdUsuarioOperador { get; set; }

    [Range(1, int.MaxValue)]
    public int Cantidad { get; set; }

    [Required, StringLength(100)]
    public string CodigoLeido { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Lote { get; set; }

    [StringLength(100)]
    public string? Serie { get; set; }

    [StringLength(300)]
    public string? Observacion { get; set; }

    public int? IdUbicacionOrigen { get; set; }
    public int? IdUbicacionDestino { get; set; }
}
