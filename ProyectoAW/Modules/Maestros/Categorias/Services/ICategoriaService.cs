using ProyectoAW.Modules.Maestros.Categorias.DTOs.Requests;
using ProyectoAW.Modules.Maestros.Categorias.DTOs.Responses;

namespace ProyectoAW.Modules.Maestros.Categorias.Services
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDto>> ListarCategoriasAsync();
        Task<CategoriaDto?> ObtenerCategoriaPorIdAsync(int id);
        Task<CategoriaDto?> ObtenerCategoriaPorNombreAsync(string nombre);
        Task<CategoriaDto> CrearCategoriaAsync(CrearCategoriaDto dto);

        Task<bool> ActualizarCategoriaAsync(int id,ActualizarCategoriaDto dto );
        Task<bool> CambiarEstadoCategoriaAsync(int id, bool activo);
    }
}
