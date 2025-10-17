using System.Linq.Expressions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BaseRepositoryReadonly<TEntity, TContext> : IBaseRepositoryReadonly<TEntity>
    where TEntity : class
    where TContext : DbContext
{
    protected readonly TContext _context;
    public BaseRepositoryReadonly(TContext context)
    {
        _context = context;
    }

    public IQueryable<TEntity> Asqueryable()
    {
        return _context.Set<TEntity>().AsQueryable();
    }
    public TEntity? Get(Expression<Func<TEntity, bool>> expression)
    {
        return _context.Set<TEntity>().FirstOrDefault(expression);
    }
    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(expression);
    }
    public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? expression = null)
    {
        if (expression is null)
        {
            return await _context.Set<TEntity>().ToListAsync();
        }
        return await _context.Set<TEntity>().Where(expression).ToListAsync();
    }
}