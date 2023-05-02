using Appraisal.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Appraisal.DAL.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        Task AddAsync(T entity);
        T Get(int id);
        IEnumerable<T> GetAll();
        Task SaveChangesAsync();
        void Update(T entity);
        void Delete(T entity);
    }
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly AppraisalDBContext dbContext;
        public BaseRepository(AppraisalDBContext dbContext) { this.dbContext = dbContext; }
        public virtual async Task AddAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
        }

        public virtual void Delete(T entity)
        {
            dbContext.Set<T>().Remove(entity);
        }
        public virtual T Get(int id)
        {
            return dbContext.Set<T>().Find(id);
        }
        public virtual IEnumerable<T> GetAll()
        {
            return dbContext.Set<T>();
        }
        public virtual async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
        public virtual void Update(T entity)
        {
            dbContext.Update<T>(entity);
        }
    }
}
