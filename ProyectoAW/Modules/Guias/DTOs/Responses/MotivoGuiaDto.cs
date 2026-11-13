namespace ProyectoAW.Modules.Guias.DTOs.Responses;

public class MotivoGuiaDto
{
    public int IdMotivoGuia { get; set; }
    public int IdTipoOperacionGuia { get; set; }
    public string TipoOperacion { get; set; } = string.Empty;
    public string NombreMotivo { get; set; } = string.Empty;
    public bool RequiereProveedor { get; set; }
    public bool RequiereCliente { get; set; }
    public bool RequiereAlmacenOrigen { get; set; }
    public bool RequiereAlmacenDestino { get; set; }
    public int IdTipoComportamiento { get; set; }
    public string TipoComportamiento { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
