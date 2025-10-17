using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBaseRepository<TEntity> : IBaseRepositoryReadonly<TEntity> where TEntity : class
    {
        Task SaveChangesAsync();
        Task CreateAsync(TEntity entity);
        Task CreateAsync(IList<TEntity> entities);
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}