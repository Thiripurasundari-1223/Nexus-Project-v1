using System.Collections.Generic;
using System.Threading.Tasks;
using Timesheet.DAL.DBContext;

namespace Timesheet.DAL.Repository
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
        private readonly TSDBContext _dbContext;
        public BaseRepository(TSDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public virtual async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }
        public virtual void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }
        public virtual T Get(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }
        public virtual IEnumerable<T> GetAll()
        {
            return _dbContext.Set<T>();
        }
        public virtual async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public virtual void Update(T entity)
        {
            _dbContext.Update<T>(entity);
        }
    }
}