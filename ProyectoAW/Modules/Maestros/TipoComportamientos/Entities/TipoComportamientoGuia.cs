using ProyectoAW.Modules.Maestros.MotivosGuia.Entities;

namespace ProyectoAW.Modules.Maestros.TipoComportamientos.Entities
{
    public class TipoComportamientoGuia
    {
        public int IdTipoComportamiento { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public ICollection<MotivoGuia> Motivos { get; set; } = new List<MotivoGuia>();

    }

    public static class TipoComportamientoGuiaIds
    {
        public const int MovimientoNormal = 1;
        public const int TrasladoInterno = 2;
    }
}
