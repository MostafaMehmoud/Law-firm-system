using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;

namespace Law.DAL.Repository.IRepository
{
    public interface ICaseRepository : IRepositoryBase<Case>
    {
        Task<Case> GetByIdWithOpinionsAsync(int caseId);

        Task<Case> GetByIdWithIncludesAsync(int id);

        Task<List<Case>> GetAllWithOffersAsync();
    }
}
