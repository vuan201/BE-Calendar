using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface IBaseRepositoryReadonly<TEntity> where TEntity : class 
{
    IQueryable<TEntity> Asqueryable();
    TEntity? Get(Expression<Func<TEntity, bool>> expression);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression);
    Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? expression = null);
}