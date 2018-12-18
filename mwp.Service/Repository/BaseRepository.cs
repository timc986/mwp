using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace mwp.Service.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DbContext dbContext;
        private bool disposed = false;

        public BaseRepository(DbContext context)
        {
            dbContext = context;
        }

        public virtual async Task<bool> Add(T entity)
        {
            try
            {
                dbContext.Set<T>().Add(entity);
                dbContext.Entry(entity).State = EntityState.Added;

                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                return await Task.FromResult(false); ;
            }
        }

        public virtual async Task<List<T>> GetAll()
        {
            return dbContext.Set<T>().ToList();
        }

        public virtual async Task<List<T>> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var result = dbContext.Set<T>().Where(i => true);

            foreach (var includeExpression in includes)
                result = result.Include(includeExpression);

            return await result.ToListAsync();
        }

        public virtual IQueryable<T> SearchBy(Expression<Func<T, bool>> searchBy, params Expression<Func<T, object>>[] includes)
        {
            var result = dbContext.Set<T>().Where(searchBy);

            foreach (var includeExpression in includes)
                result = result.Include(includeExpression);

            return result.AsQueryable();
        }

        public virtual async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return await dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<bool> Update(T entity)
        {
            try
            {
                dbContext.Set<T>().Attach(entity);
                dbContext.Entry(entity).State = EntityState.Modified;

                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                return await Task.FromResult(false);
            }
        }

        public virtual async Task<bool> Delete(Expression<Func<T, bool>> identity, params Expression<Func<T, object>>[] includes)
        {
            var results = dbContext.Set<T>().Where(identity);

            foreach (var includeExpression in includes)
                results = results.Include(includeExpression);
            try
            {
                dbContext.Set<T>().RemoveRange(results);
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                return await Task.FromResult(false);
            }
        }

        public virtual async Task<bool> Delete(T entity)
        {
            dbContext.Set<T>().Remove(entity);
            return await Task.FromResult(true);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            disposed = true;
        }
    }
}
