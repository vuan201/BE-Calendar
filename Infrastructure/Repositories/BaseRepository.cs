using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BaseRepository<TEntity, TContext> : BaseRepositoryReadonly<TEntity, TContext>, IBaseRepository<TEntity>
    where TEntity : class
    where TContext : DbContext
{
    public BaseRepository(TContext context) : base(context) { }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task CreateAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }
    public async Task CreateAsync(IList<TEntity> entities)
    {
        if (entities != null && entities.Count == 0)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
        }
    }
    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }
    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        _context.Entry(entity).State = EntityState.Deleted;
    }
}