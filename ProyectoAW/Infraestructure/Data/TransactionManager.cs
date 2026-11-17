using ProyectoAW.Infrastructure.Data;

public class TransactionManager : ITransactionManager
{
    private readonly AppDbContext _context;

    public TransactionManager(AppDbContext context)
    {
        _context = context;
    }

    public async Task ExecuteAsync(
        Func<CancellationToken, Task> operation,
        CancellationToken ct = default)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(ct);

        await operation(ct);

        await tx.CommitAsync(ct);
    }

    public async Task ExecuteSerializableAsync(
        Func<CancellationToken, Task> operation,
        CancellationToken ct = default)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(
            ct);

        await operation(ct);

        await tx.CommitAsync(ct);
    }
}