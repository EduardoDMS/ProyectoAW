using ProjectAW.Modules.Stocks.Services;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Seguridad.Security.HttpAuth;
using ProyectoAW.Modules.Stocks.Repositories;

namespace ProyectoAW.Modules.Stocks.Validacion
{
    public class ValidacionesBase
    {
        public readonly ILecturaGuiaRepository _repository;
        public readonly ICurrentUserService _currentUser;
        public readonly ITransactionManager _transactions;
        public readonly IReadOnlyDictionary<int, IGuiaStockProcessor> _stockProcessors;

        public ValidacionesBase(
                                    ILecturaGuiaRepository repository,
                                    ICurrentUserService currentUser,
                                    ITransactionManager transactions,
                                    IEnumerable<IGuiaStockProcessor> stockProcessors)
        {
            _repository = repository;
            _currentUser = currentUser;
            _transactions = transactions;
            _stockProcessors = stockProcessors.ToDictionary(p => p.IdTipoOperacionGuia);
        }
    }
}
