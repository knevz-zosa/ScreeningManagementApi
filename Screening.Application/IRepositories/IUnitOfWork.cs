using Screening.Domain.Contracts;

namespace Screening.Application.IRepositories;
public interface IUnitOfWork<TId> : IDisposable
{
    IWriteRepositoryAsync<T, TId> WriteRepositoryFor<T>() where T : BaseEntity<TId>;
    IReadRepositoryAsync<T, TId> ReadRepositoryFor<T>() where T : BaseEntity<TId>;
    Task<int> CommitAsync(CancellationToken cancellationToken);
}

