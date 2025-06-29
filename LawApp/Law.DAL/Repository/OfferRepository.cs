using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Law.DAL.Data;
using Law.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Law.DAL.Repository
{
    public class OfferRepository : RepositoryBase<Offer>, IOfferRepository
    {
        public OfferRepository(LawAppDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsAsync(Expression<Func<Offer, bool>> predicate)
        {
            return await _context.offers.AnyAsync(predicate);
        }
        public async Task<Offer> GetOfferByUserAndCaseAsync(string userId, int caseId)
        {
            return await _context.offers
                .FirstOrDefaultAsync(o => o.UserId == userId && o.CaseId == caseId);
        }
        public async Task<List<Offer>> GetOffersByUserAsync(string userId)
        {
            return await _context.offers
                .Include(o => o.Case) // Include القضية المرتبطة
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

    }
}
