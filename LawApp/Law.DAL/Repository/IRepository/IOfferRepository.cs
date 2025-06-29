using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;

namespace Law.DAL.Repository.IRepository
{
    public interface IOfferRepository : IRepositoryBase<Offer>
    {
        Task<List<Offer>> GetOffersByUserAsync(string userId);

        Task<bool> ExistsAsync(Expression<Func<Offer, bool>> predicate);
        Task<Offer> GetOfferByUserAndCaseAsync(string userId, int caseId);

    }
}
