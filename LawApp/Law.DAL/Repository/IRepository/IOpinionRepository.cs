using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;

namespace Law.DAL.Repository.IRepository
{
    public interface IOpinionRepository : IRepositoryBase<Opinion>
    {
        Task<List<Opinion>> GetAllWithUserAndCaseAsync();
        Task<List<Opinion>> GetOpinionsByUserIdAsync(string userId);
        Task<bool> ExistsAsync(Expression<Func<Opinion, bool>> predicate);
        Task<List<Opinion>> GetOpinionsByCaseIdAsync(int caseId);
        Task<Opinion> GetByIdAsync(int id);

    }
}
