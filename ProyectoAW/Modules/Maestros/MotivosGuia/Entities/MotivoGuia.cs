using ProyectoAW.Modules.Maestros.Operaciones.Entities;
using ProyectoAW.Modules.Maestros.TipoComportamientos.Entities;

namespace ProyectoAW.Modules.Maestros.MotivosGuia.Entities
{
    public class MotivoGuia
    {
        public int IdMotivoGuia { get; set; }
        public int IdTipoOperacionGuia { get; set; } = TipoOperacionGuiaIds.Entrada;

        public string NombreMotivo { get; set; } = string.Empty;
        public bool RequiereProveedor { get; set; }
        public bool RequiereCliente { get; set; }
        public bool RequiereAlmacenOrigen { get; set; }
        public bool RequiereAlmacenDestino { get; set; }
        public int IdTipoComportamiento { get; set; }
        public bool Activo { get; set; } = true;

        public TipoOperacionGuia? TipoOperacionGuia { get; set; }
        public TipoComportamientoGuia? TipoComportamiento { get; set; }



    }
}
