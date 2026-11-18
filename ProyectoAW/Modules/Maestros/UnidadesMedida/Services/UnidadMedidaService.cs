using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Modules.Maestros.UnidadesMedida.DTOs.Requests;
using ProyectoAW.Modules.Maestros.UnidadesMedida.DTOs.Responses;
using ProyectoAW.Modules.Maestros.UnidadesMedida.Entities;
using ProyectoAW.Modules.Maestros.UnidadesMedida.Repositories;

namespace ProyectoAW.Modules.Maestros.UnidadesMedida.Services
{
    public class UnidadMedidaService : IUnidadMedidaService
    {
        private readonly IUnidadMedidaRepository _repository;

        public UnidadMedidaService(IUnidadMedidaRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarUnidadMedidaDto dto)
        {
            var unidadMedida = await _repository.ObtenerPorIdAsync(id);
            if (unidadMedida == null)
                throw new NegocioException("No se encontro La unidad de medida que buscabas");

            var codigo = dto.Codigo.Trim();
            var UnidadDeMedidaConMismoCodigo = await _repository.ObtenerPorCodigoAsync(codigo);
            if(UnidadDeMedidaConMismoCodigo != null && UnidadDeMedidaConMismoCodigo.IdUnidadMedida!= id)
            {
                throw new NegocioException("Ya existe una unidad de medida con ese codigo ");
            }
            unidadMedida.Codigo = codigo;
            unidadMedida.Nombre = dto.Nombre?.Trim();
            unidadMedida.Activo = dto.Activo;

            await _repository.ActualizarAsync(unidadMedida);
            return true;

        }

        public async Task<bool> CambiarEstadoAsync(int id, bool Activo)
        {
            var unidadMedida = await _repository.ObtenerPorIdAsync(id);

            if (unidadMedida == null)
                throw new NegocioException("No se logro encontrar el producto");

            unidadMedida.Activo = Activo;
            await _repository.ActualizarAsync(unidadMedida);

            return true;
        }

        public async Task<UnidadMedidaDTO?> CrearAsync(CrearUnidadMedidaDto dto)
        {
            var codigo = dto.Codigo.Trim();
            
            var unidadDeMedidaExiste = await _repository.ObtenerPorCodigoAsync(codigo);

            if (unidadDeMedidaExiste != null)
                throw new NegocioException("Ya existe una unidad de medida con ese codigo");

            var unidadMedida = new UnidadMedida
            {
                Codigo = codigo,
                Nombre = dto.Nombre?.Trim(),
                Activo = true
            };

            await _repository.CrearAsync(unidadMedida);

            return new UnidadMedidaDTO
            {
                IdUnidadUnidad = unidadMedida.IdUnidadMedida,
                Codigo = unidadMedida.Codigo,
                Nombre = unidadMedida.Nombre,
                Activo = unidadMedida.Activo
            };
        }

        public async Task EliminarUnidadMedidaAsync(int id)
        {
            var unidadMedida = await _repository.ObtenerPorIdAsync(id);

            if (unidadMedida == null)
                throw new NegocioException("No se encontro una unidad de medida con ese id");

            var ProductosAsociados = await _repository.ValidarProductosAsociados(id);

            if (ProductosAsociados)
                throw new NegocioException("No sepuede eliminar tiene prodcutos que aun estan asociados a esta Unidad de Medida");

            await _repository.EliminarUnidadMedidaAsync(unidadMedida);
        }

        public async Task<UnidadMedidaDTO?> ObtenerPorCodigoAsync(string codigo)
        {
            var unidadMedida = await _repository.ObtenerPorCodigoAsync(codigo);

            if (unidadMedida == null)
                throw new NegocioException(
                    $"No se pudo encontrar la unidad de medida con el código: {codigo}");

            return new UnidadMedidaDTO
            {
                IdUnidadUnidad = unidadMedida.IdUnidadMedida,
                Codigo = unidadMedida.Codigo,
                Nombre = unidadMedida.Nombre,
                Activo = unidadMedida.Activo
            };
        }

        public async Task<UnidadMedidaDTO?> ObtenerPorIdAsync(int id)
        {
            var unidadMedida = await _repository.ObtenerPorIdAsync(id);

            if (unidadMedida == null)
                throw new NegocioException("Unidad de medida no localizada/encontrada. no existe");

            return new UnidadMedidaDTO
            {
                IdUnidadUnidad = unidadMedida.IdUnidadMedida,
                Nombre = unidadMedida.Nombre,
                Codigo = unidadMedida.Codigo,
                Activo = unidadMedida.Activo
            };

        }

        public async Task<List<UnidadMedidaDTO>> ObtenerTodasAsync()
        {
            var unidadMedida = await _repository.ObtenerTodas();

            return unidadMedida.Select(u => new UnidadMedidaDTO
            {
                IdUnidadUnidad = u.IdUnidadMedida,
                Nombre = u.Nombre,
                Codigo = u.Codigo,
                Activo = u.Activo
            }).ToList();
        }
    }
}
