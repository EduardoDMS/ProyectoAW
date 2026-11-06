namespace ProyectoAW.Models
{
    public class NoEncontradoException : Exception
    {
        public NoEncontradoException(string mensaje) : base(mensaje) { }
    }
}
