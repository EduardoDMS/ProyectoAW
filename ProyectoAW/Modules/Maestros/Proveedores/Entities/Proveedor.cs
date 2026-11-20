using ProyectoAW.Modules.Maestros.TipoDocumentos.Entities;

namespace ProyectoAW.Modules.Maestros.Proveedores.Entities
{
    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public string RazonSocial { get; set; } = string.Empty;
        public int IdTipoDocumento { get; set; }
        public string NumeroDocumento { get; set; } = string.Empty;
        public string? Direccion { get; set; }
        public string? NombreContacto { get; set; }
        public string? CorreoContacto { get; set; }
        public string? TelefonoContacto { get; set; }      
        public bool Activo { get; set; }
        public string FchRegistro { get; set; } = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);
        public string? FchModificacion { get; set; }

        public TipoDocumento TipoDocumento { get; set; } = null!;
    }
}
