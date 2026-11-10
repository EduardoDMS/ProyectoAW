using ProyectoAW.Modules.Maestros.Ubicaciones.DTOs.Requests;
using ProyectoAW.Modules.Maestros.Ubicaciones.DTOs.Responses;
using ProyectoAW.Modules.Maestros.Ubicaciones.Repositories;
using ProyectoAW.Modules.Maestros.Ubicaciones.Entities;
using ProyectoAW.Infraestructure.RP;


namespace ProyectoAW.Modules.Maestros.Ubicaciones.Services
{
    public class UbicacionService : IUbicacionService
    {
        private readonly IUbicacionRepository _repository;

        public UbicacionService(IUbicacionRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarUbicacionDto dto)
        {
            var ubicacion = await _repository.ObtenerUbicacionPorIdAsync(id);
            
            if(ubicacion == null)
                return false;

            var codigo = dto.Codigo.Trim();

            var ubicacionConElMismoCodigo = await _repository.ObtenerUbicacionPorCodigoAsync(codigo);
            if(ubicacionConElMismoCodigo != null && ubicacionConElMismoCodigo.IdUbicacion != id)
            {
                throw new NegocioException($"Ya existe una ubicación con el código '{codigo}'.");
            }

            ubicacion.Codigo = codigo;
            ubicacion.Descripcion = dto.Descripcion?.Trim();
            ubicacion.Activo = dto.Activo;
            ubicacion.FchModificacion = DateTime.Now;
            ubicacion.IdAlmacen = dto.IdAlmacen;

            await _repository.ActualizarUbicacionAsync(ubicacion);

            return true;

        }

        public async Task<UbicacionDto> CrearUbicacionAsync(CrearUbicacionDto dto)
        {
            var codigo = dto.Codigo.Trim();

            var ubicacionExistente = await _repository.ObtenerUbicacionPorCodigoAsync(codigo);
        
            if(ubicacionExistente != null)
                throw new  NegocioException($"Ya existe una ubicación con el código '{codigo}'.");

            var ubicacion = new Ubicacion
            {
                Codigo = codigo,
                Descripcion = dto.Descripcion?.Trim(),
                Activo = dto.Activo,
                FchRegistro = DateTime.Now,
                IdAlmacen = dto.IdAlmacen
            };

            await _repository.CrearUbicacionAsync(ubicacion);

            return new UbicacionDto
            {
                IdUbicacion = ubicacion.IdUbicacion,
                Codigo = ubicacion.Codigo,
                Descripcion = ubicacion.Descripcion,
                Activo = ubicacion.Activo,
                IdAlmacen = ubicacion.IdAlmacen,
                EsUbicacionDespacho = ubicacion.UbicacionesDespacho.Any(x => x.IdAlmacen == ubicacion.IdAlmacen)
            };
        }

        public async Task<bool> CambiarEstadoAsync(int id, bool activo)
        {
            var ubicacion = await _repository.ObtenerUbicacionPorIdAsync(id);
            
            if (ubicacion == null)
                throw new NegocioException($"No se encontró la ubicación con ID {id}.");

            ubicacion.Activo = activo;
            ubicacion.FchModificacion = DateTime.UtcNow;// estaria buena evaluar aqui el cambio tambien de fecha 

            await _repository.ActualizarUbicacionAsync(ubicacion);
            return true;
        }

        public async Task<List<UbicacionDto>> ObtenerUbicacionesAsync()
        {
            var ubicaciones = await _repository.ListarUbicacionesAsync();
            return ubicaciones.Select(u => new UbicacionDto
            {
                IdUbicacion = u.IdUbicacion,
                Codigo = u.Codigo,
                Descripcion = u.Descripcion,
                Activo = u.Activo,
                IdAlmacen = u.IdAlmacen,
                EsUbicacionDespacho = u.UbicacionesDespacho.Any(x => x.IdAlmacen == u.IdAlmacen)
            }).ToList();

        }

        public async Task<List<UbicacionDto>> ObtenerUbicacionesAsync(int? idAlmacen, bool soloActivas)
        {
            var ubicaciones = await _repository.ListarUbicacionesAsync(idAlmacen, soloActivas);
            return ubicaciones.Select(u => new UbicacionDto { IdUbicacion = u.IdUbicacion, Codigo = u.Codigo, Descripcion = u.Descripcion, Activo = u.Activo, EsUbicacionDespacho = u.UbicacionesDespacho.Any(x => x.IdAlmacen == u.IdAlmacen), IdAlmacen = u.IdAlmacen }).ToList();
        }

        public async Task<UbicacionDto?> ObtenerUbicacionPorCodigoAsync(string codigo)
        {
            var ubicacion = await _repository.ObtenerUbicacionPorCodigoAsync(codigo);

            if (ubicacion == null)
                throw new NegocioException($"No se encontró la ubicación con código {codigo}.");

            return new UbicacionDto
            {
                IdUbicacion = ubicacion.IdUbicacion,
                Codigo = ubicacion.Codigo,
                Descripcion = ubicacion.Descripcion,
                Activo = ubicacion.Activo,
                IdAlmacen = ubicacion.IdAlmacen,
                EsUbicacionDespacho = ubicacion.UbicacionesDespacho.Any(x => x.IdAlmacen == ubicacion.IdAlmacen)
            };
        }

        public async Task<UbicacionDto?> ObtenerUbicacionPorIdAsync(int id)
        {
            var ubicacion = await _repository.ObtenerUbicacionPorIdAsync(id);

            if(ubicacion == null)
                throw new NegocioException($"No se encontró la ubicación con ID {id}.");
            return new UbicacionDto
            {
                IdUbicacion = ubicacion.IdUbicacion,
                Codigo = ubicacion.Codigo,
                Descripcion = ubicacion.Descripcion,
                Activo = ubicacion.Activo,
                IdAlmacen = ubicacion.IdAlmacen,
                EsUbicacionDespacho = ubicacion.UbicacionesDespacho.Any(x => x.IdAlmacen == ubicacion.IdAlmacen)
            };

        }
    }
}
