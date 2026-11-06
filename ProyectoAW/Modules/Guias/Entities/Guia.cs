using ProyectoAW.Modules.Maestros.Estados.Entities;
using ProyectoAW.Modules.Maestros.Operaciones.Entities;
using ProyectoAW.Modules.Maestros.Proveedores.Entities;

namespace ProyectoAW.Modules.Guias.Entities;

public class Guia
{
    public int IdGuia { get; set; }
    public string NumeroGuia { get; set; } = string.Empty;

    public int IdTipoOperacionGuia { get; set; } = TipoOperacionGuiaIds.Entrada;
    public TipoOperacionGuia TipoOperacion { get; set; } = null!;

    public int IdEstadoGuia { get; set; } = EstadoGuiasIds.Pendiente;
    public EstadoGuia Estado { get; set; } = null!;

    public int IdProveedor { get; set; }
    public Proveedor Proveedor { get; set; } = null!;

    public DateTime FechaProgramada { get; set; }
    public string? Observacion { get; set; }
    public DateTime FechaRegistro { get; set; }

    public ICollection<DetalleGuia> Detalles { get; set; } = new List<DetalleGuia>();
}
