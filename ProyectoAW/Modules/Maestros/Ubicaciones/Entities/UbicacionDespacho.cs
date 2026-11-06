using ProyectoAW.Modules.Maestros.Almacenes.Entities;

namespace ProyectoAW.Modules.Maestros.Ubicaciones.Entities;

/// <summary>Vincula una ubicación existente con un almacén que la habilita para despacho.</summary>
public sealed class UbicacionDespacho
{
    public int IdAlmacen { get; set; }
    public int IdUbicacion { get; set; }
    public Almacen? Almacen { get; set; }
    public Ubicacion? Ubicacion { get; set; }
}
