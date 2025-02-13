using Screening.Application.IRepositories;
using Screening.Domain.Contracts;
using Screening.Infrastructure.Context;
using System.Collections.Concurrent;

namespace Screening.Infrastructure.Repositories;
public class UnitOfWork<TId> : IUnitOfWork<TId>
{
    private readonly ApplicationDbContext _context;
    private readonly ConcurrentDictionary<string, object> _repositories = new();
    private bool _disposed;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public IReadRepositoryAsync<T, TId> ReadRepositoryFor<T>() where T : BaseEntity<TId>
    {
        var repositoryKey = $"{typeof(T).Name}_Read";

        var lazyRepository = _repositories.GetOrAdd(repositoryKey, _ =>
        {
            var repositoryType = typeof(ReadRepositoryAsync<,>);
            var repositoryInstance = Activator.CreateInstance(
                repositoryType.MakeGenericType(typeof(T), typeof(TId)), _context);
            return new Lazy<IReadRepositoryAsync<T, TId>>(() => (IReadRepositoryAsync<T, TId>)repositoryInstance);
        });

        return ((Lazy<IReadRepositoryAsync<T, TId>>)lazyRepository).Value;
    }

    public IWriteRepositoryAsync<T, TId> WriteRepositoryFor<T>() where T : BaseEntity<TId>
    {
        var repositoryKey = $"{typeof(T).Name}_Write";

        var lazyRepository = _repositories.GetOrAdd(repositoryKey, _ =>
        {
            var repositoryType = typeof(WriteRepositoryAsync<,>);
            var repositoryInstance = Activator.CreateInstance(
                repositoryType.MakeGenericType(typeof(T), typeof(TId)), _context);
            return new Lazy<IWriteRepositoryAsync<T, TId>>(() => (IWriteRepositoryAsync<T, TId>)repositoryInstance);
        });

        return ((Lazy<IWriteRepositoryAsync<T, TId>>)lazyRepository).Value;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }
    }
}

