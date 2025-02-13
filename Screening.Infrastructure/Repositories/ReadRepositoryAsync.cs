using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Domain.Contracts;
using Screening.Infrastructure.Context;

namespace Screening.Infrastructure.Repositories;
public class ReadRepositoryAsync<T, TId> : IReadRepositoryAsync<T, TId> where T : BaseEntity<TId>
{
    private readonly ApplicationDbContext _context;

    public ReadRepositoryAsync(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<T> Entities => _context.Set<T>();

    public async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<T> GetAsync(TId id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
}
