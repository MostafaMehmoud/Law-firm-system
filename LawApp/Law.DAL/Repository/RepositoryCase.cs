using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Law.DAL.Data;
using Law.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Law.DAL.Repository
{
    public class RepositoryCase : RepositoryBase<Case>, ICaseRepository
    {

        public RepositoryCase(LawAppDbContext context) : base(context)
        {
        }
        public async Task<Case> GetByIdWithIncludesAsync(int id)
        {
            var caseWithOpinions = await _context.cases
                .Include(c => c.Opinions)
                .FirstOrDefaultAsync(c => c.Id == id);

            return caseWithOpinions;
        }


        public async Task<List<Case>> GetAllWithOffersAsync()
        {
            return await _context.cases
                .Include(c => c.Offers)
                .ToListAsync();
        }
        public async Task<Case> GetByIdWithOpinionsAsync(int caseId)
        {
            return await _context.cases
                .Include(c => c.Opinions)
                .ThenInclude(o => o.User) // لو تحب تعرض اسم المحامي من Identity
                .FirstOrDefaultAsync(c => c.Id == caseId);
        }

    }
}