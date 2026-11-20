namespace ProyectoAW.Infrastructure.Data;

/// <summary>Ejecuta operaciones de aplicación dentro de una transacción.</summary>
public interface ITransactionManager
{
    Task ExecuteAsync(Func<CancellationToken, Task> operation, CancellationToken ct = default);
    Task ExecuteSerializableAsync(Func<CancellationToken, Task> operation, CancellationToken ct = default);
}
