using ProyectoAW.Modules.Maestros.Estados.Entities;
using ProyectoAW.Modules.Maestros.Operaciones.Entities;
using ProyectoAW.Modules.Maestros.Proveedores.Entities;
using ProyectoAW.Modules.Maestros.Almacenes.Entities;
using ProyectoAW.Modules.Maestros.Clientes.Entities;
using ProyectoAW.Modules.Stocks.Entities;

namespace ProyectoAW.Modules.Guias.Entities;

public class Guia
{
    public int IdGuia { get; set; }
    public string NumeroGuia { get; set; } = string.Empty;

    public int IdUsuarioRegistro { get; set; }
    public int? IdUsuarioModificacion { get; set; }


    public int IdTipoOperacionGuia { get; set; } = TipoOperacionGuiaIds.Entrada;
    public TipoOperacionGuia TipoOperacion { get; set; } = null!;

    public int IdEstadoGuia { get; set; } = EstadoGuiasIds.Pendiente;
    public EstadoGuia Estado { get; set; } = null!;

    public int? IdMotivoGuia { get; set; }
    public MotivoGuia? Motivo { get; set; }

    public int? IdProveedor { get; set; }
    public Proveedor? Proveedor { get; set; }

    public int? IdCliente { get; set; }
    public Cliente? Cliente { get; set; }

    public int? IdAlmacenOrigen { get; set; }
    public Almacen? AlmacenOrigen { get; set; }

    public int? IdAlmacenDestino { get; set; }
    public Almacen? AlmacenDestino { get; set; }

    //se esta auntando a si mismo para la guia relacion
    public int? IdGuiaRelacionada { get; set; }
    public Guia? GuiaRelacionada { get; set; }

    public DateTime FechaProgramada { get; set; }
    public string? Observacion { get; set; }
    public DateTime FechaRegistro { get; set; }
    public DateTime? FechaActualizada { get; set; }

    public string TipoDocumentoReferencia { get; set; } = string.Empty;
    public string NumeroDocumentoReferencia { get; set; } = string.Empty;

    public ICollection<DetalleGuia> Detalles { get; set; } = new List<DetalleGuia>();
    public ICollection<MovimientoStock>? MovimientoStocks { get; set; }
    public ICollection<Guia> GuiasRelacionadas { get; set; } = new List<Guia>();
}
