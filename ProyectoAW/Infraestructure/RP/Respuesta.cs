namespace ProyectoAW.Infraestructure.RP
{
    public class Respuesta
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        ///  para que sirve object? 
        ///  almacena cualquier tipo de valor bajo una misma referenvia, lo que permite que se pueda almacenar cualquier tipo de valor en la propiedad Datos.
        public object? Datos { get; set; }
    }
}
