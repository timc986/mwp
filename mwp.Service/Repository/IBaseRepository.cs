using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace mwp.Service.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<bool> Add(T entity);

        Task<List<T>> GetAll();

        Task<List<T>> GetAll(params Expression<Func<T, object>>[] includes);

        IQueryable<T> SearchBy(Expression<Func<T, bool>> searchBy, params Expression<Func<T, object>>[] includes);

        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        Task<bool> Update(T entity);

        Task<bool> Delete(Expression<Func<T, bool>> identity, params Expression<Func<T, object>>[] includes);

        Task<bool> Delete(T entity);

    }
}