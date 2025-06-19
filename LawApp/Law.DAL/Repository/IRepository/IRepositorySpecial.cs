using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.DAL.Repository.IRepository
{
    public interface IRepositorySpecial<T> : IRepositoryBase<T> where T : class
    {
        Task<T> GetMin();
        Task<T> GetMax();
        Task<T> GetNextOrPreviousItemByCode(int id, string direction);
        int GetMaxIdOfItem();
        Task<int> GetNewCode();
    }
}
