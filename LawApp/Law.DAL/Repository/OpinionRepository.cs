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
    public class OpinionRepository : RepositoryBase<Opinion>, IOpinionRepository
    {
        public OpinionRepository(LawAppDbContext context) : base(context)
        {
        }
        public async Task<List<Opinion>> GetAllWithUserAndCaseAsync()
        {
            return await _context.opinsion
                .Include(o => o.User)
                .Include(o => o.Case)
                .OrderByDescending(o => o.PostedAt)
                .ToListAsync();
        }
        public async Task<List<Opinion>> GetOpinionsByUserIdAsync(string userId)
        {
            return await _context.opinsion
                .Where(o => o.UserId == userId)
                .Include(o => o.User)
                .Include(o => o.Case)
                .OrderByDescending(o => o.PostedAt)
                .ToListAsync();
        }
        public async Task<bool> ExistsAsync(Expression<Func<Opinion, bool>> predicate)
        {
            return await _context.opinsion.AnyAsync(predicate);
        }
        public async Task<Opinion> GetByIdAsync(int id)
        {
            return await _context.opinsion.FindAsync(id);
        }

        public async Task<List<Opinion>> GetOpinionsByCaseIdAsync(int caseId)
        {
            return await _context.opinsion
                .Where(o => o.CaseId == caseId)
                .Include(o => o.User)
                .OrderByDescending(o => o.PostedAt)
                .ToListAsync();
        }
    }
}
