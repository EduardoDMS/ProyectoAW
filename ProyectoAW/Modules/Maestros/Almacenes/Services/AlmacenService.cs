using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Modules.Maestros.Almacenes.DTOs.Requests;
using ProyectoAW.Modules.Maestros.Almacenes.DTOs.Responses;
using ProyectoAW.Modules.Maestros.Almacenes.Entities;
using ProyectoAW.Modules.Maestros.Almacenes.Repositories;
//using ProyectoAW  .Infrastructure.Data;

namespace ProyectoAW.Modules.Maestros.Almacenes.Services
{
    public class AlmacenService : IAlmacenService
    { 
        private readonly IAlmacenRepository _repository;
        public AlmacenService(IAlmacenRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarAlmacenDto dto)
        {
            var almacen = await _repository.ObtenerAlmacenPorIdAsync(id);

            if (almacen == null)
                throw new NegocioException("No se encontro un almacen con ese id");

            var codigo = dto.Codigo.Trim();
            await ValidarCodigoUnicoAsync(id, codigo);

            var ubicacionesSeleccionadas = (dto.IdUbicacionesDespacho ?? [])
                .Distinct()
                .ToArray();
            await ValidarUbicacionesDespachoAsync(id, ubicacionesSeleccionadas.ToHashSet());

            almacen.Codigo = codigo;
            almacen.Nombre = dto.Nombre?.Trim();
            almacen.Activo = dto.Activo;
            almacen.FchModificacion = DateTime.UtcNow;

            await _repository.ActualizarAlmacenYUbicacionesDespachoAsync(
                almacen, ubicacionesSeleccionadas);
            return true;
        }

        public async Task<bool> CambiarEstadoAlmacenAsync(int id, bool activo)
        {
            var almacen = await _repository.ObtenerAlmacenPorIdAsync(id);

            /// se coloca el termino new al llamar a la clase por que se esta llamando a la clase NegocioException
            /// que es una clase personalizada para manejar excepciones de negocio en la aplicación.
            /// Al usar new, se crea una nueva instancia de la clase NegocioException con un mensaje específico que indica que no se encontró el almacén con el ID proporcionado.
            if (almacen == null)
                throw new NegocioException($"No se encontró el almacén con ID {id}.");

            almacen.Activo = activo;
            almacen.FchModificacion = DateTime.UtcNow;
            await _repository.ActualizarAlmacenAsync(almacen);

            return true;
        }
        //  // REFACTORIZAR ESTE METODO PARA QUE NO SEA TAN LARGO Y TENGA MENOS RESPONSABILIDADES
       
        public async Task<AlmacenDTO> CrearAlmacenAsync(CrearAlmacenDto dto)
        {
            if (dto.IdUbicacionesDespacho?.Count > 0)
                throw new NegocioException("Primero crea el almacén y sus ubicaciones; después podrás asignar las ubicaciones de despacho al editarlo.");
            var codigo = dto.Codigo.Trim();
            var almacenExistente = await _repository.ObtenerAlmacenPorCodigoAsync(codigo);

            if(almacenExistente != null)
                throw new NegocioException("El código del almacén ya existe.");

            var almacen = new Almacen
            {
                Codigo = codigo,
                Nombre = dto.Nombre?.Trim(),
                Activo = dto.Activo,
                FchRegistro = DateTime.UtcNow
            };

            await _repository.CrearAlmacenAsync(almacen);

            return new AlmacenDTO
            {
                IdAlmacen = almacen.IdAlmacen,
                Codigo = almacen.Codigo,
                Nombre = almacen.Nombre,
                Activo = almacen.Activo,
                FchRegistro = almacen.FchRegistro,
                IdUbicacionesDespacho = []
            };

        }

        public async Task<List<AlmacenDTO>> ObtenerAlmacenesTodosAsync()
        {
            var almacenes = await _repository.ObtenerAlmacenesAsync();
            
            return almacenes.Select(a => new AlmacenDTO
            {
                IdAlmacen = a.IdAlmacen,
                Codigo = a.Codigo,
                Nombre = a.Nombre,
                Activo = a.Activo,
                FchRegistro = a.FchRegistro,
                IdUbicacionesDespacho = a.UbicacionesDespacho.Select(u => u.IdUbicacion).ToList()
            }).ToList();
        }

        public async Task<AlmacenDTO?> ObtenerAlmacenPorCodigoAsync(string codigo)
        {
            var almacen = await _repository.ObtenerAlmacenPorCodigoAsync(codigo);

            if (almacen == null)
                throw new NegocioException($" El almacen con el codigo solicitado {codigo} no existe.");

            return new AlmacenDTO
            {
                IdAlmacen = almacen.IdAlmacen,
                Codigo = almacen.Codigo,
                Nombre = almacen.Nombre,
                Activo = almacen.Activo,
                FchRegistro = almacen.FchRegistro,
                IdUbicacionesDespacho = almacen.UbicacionesDespacho.Select(u => u.IdUbicacion).ToList()
            };
        }

        public async Task<AlmacenDTO?> ObtenerAlmacenPorIdAsync(int id)
        {
             var almacen = await _repository.ObtenerAlmacenPorIdAsync(id);

            if (almacen == null)
                throw new NegocioException($"El almacen con el id de {id} no existe");

             return new AlmacenDTO
             {
                 IdAlmacen = almacen.IdAlmacen,
                 Codigo = almacen.Codigo,
                 Nombre = almacen.Nombre,
                 Activo = almacen.Activo,
                 FchRegistro = almacen.FchRegistro,
                 IdUbicacionesDespacho = almacen.UbicacionesDespacho.Select(u => u.IdUbicacion).ToList()
             };
        }

        private async Task ValidarCodigoUnicoAsync(int idAlmacen, string codigo)
        {
            var almacenExistente = await _repository.ObtenerAlmacenPorCodigoAsync(codigo);
            if (almacenExistente is not null && almacenExistente.IdAlmacen != idAlmacen)
                throw new NegocioException("El código del almacén ya existe.");
        }

        private async Task ValidarUbicacionesDespachoAsync(
            int idAlmacen, IReadOnlySet<int> idsSeleccionados)
        {
            var idsValidos = await _repository.ListarUbicacionesDelAlmacenAsync(
                idAlmacen, idsSeleccionados);
            if (idsSeleccionados.Except(idsValidos).Any())
                throw new NegocioException("Selecciona únicamente ubicaciones pertenecientes a este almacén.");
        }

    }
}
