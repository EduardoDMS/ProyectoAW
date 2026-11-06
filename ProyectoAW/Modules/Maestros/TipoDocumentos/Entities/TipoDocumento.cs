using ProyectoAW.Modules.Maestros.Clientes.Entities;
using ProyectoAW.Modules.Maestros.Proveedores.Entities;

namespace ProyectoAW.Modules.Maestros.TipoDocumentos.Entities
{
    public class TipoDocumento
    {
        public int IdTipoDocumento {  get; set; }
        public string Documento { get; set; } = string.Empty; // RUC, DNI, CE, ...

        public ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
        public ICollection<Proveedor> Proveedores { get; set; } = new List<Proveedor>();
    }
}
