using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Law.DAL.Repository.IRepository
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(int id);

        void AddRange(IEnumerable<T> entities);

        void UpdateRange(IEnumerable<T> entities);
        void DeleteRange(IEnumerable<T> entities);
        void Save();
        Task<IEnumerable<T>> GetAllWithInclude(params Expression<Func<T, object>>[] includeProperties);


    }
}
