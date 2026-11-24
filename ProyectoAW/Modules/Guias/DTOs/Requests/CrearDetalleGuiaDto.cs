using System.ComponentModel.DataAnnotations;

namespace ProyectoAW.Modules.Guias.DTOs.Requests;

public class CrearDetalleGuiaDto
{
    public int IdProducto { get; set; }

    public decimal Cantidad { get; set; }
}
