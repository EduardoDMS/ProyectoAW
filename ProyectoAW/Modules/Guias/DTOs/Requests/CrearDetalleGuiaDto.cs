using System.ComponentModel.DataAnnotations;

namespace ProyectoAW.Modules.Guias.DTOs.Requests;

public class CrearDetalleGuiaDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un producto.")]
    public int IdProducto { get; set; }

    [Range(typeof(decimal), "0.01", "9999999999999999", ErrorMessage = "La cantidad debe ser mayor que cero.")]
    public decimal Cantidad { get; set; }
}
