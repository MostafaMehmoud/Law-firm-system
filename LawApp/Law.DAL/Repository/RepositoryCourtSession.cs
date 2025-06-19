using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Law.CORE.Entities;
using Law.DAL.Data;
using Law.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Law.DAL.Repository
{
    public class RepositoryCourtSession : RepositoryBase<CourtSession>, IRepositorySpecial<CourtSession>
    {
        public RepositoryCourtSession(LawAppDbContext context) : base(context)
        {
        }

        public async Task<CourtSession> GetMax()
        {
            var maxStage = await _context.courtsSessions.OrderByDescending(n => n.Code).FirstOrDefaultAsync();
            return maxStage;
        }

        public int GetMaxIdOfItem()
        {
            var id = _context.courtsSessions.Max(i => i.Id);
            return id;
        }

        public async Task<CourtSession> GetMin()
        {
            var minSatge = await _context.courtsSessions.OrderBy(n => n.Code).FirstOrDefaultAsync();
            return minSatge;
        }

        public async Task<int> GetNewCode()
        {
            try
            {
                // Use DefaultIfEmpty(0) to handle an empty table.
                int? maxCode = await _context.courtsSessions
                    .Select(c => (int?)c.Code)

                    .MaxAsync();


                return (maxCode ?? 0) + 1;
            }
            catch (Exception ex)
            {
                // Log the full exception details for debugging.
                Console.WriteLine("Exception in Repository.GetNewCode: " + ex.ToString());
                throw;  // Rethrow to let the upper layers catch it.
            }
        }

        public async Task<CourtSession> GetNextOrPreviousItemByCode(int id, string direction)
        {
            // Get the current record's code.
            var currentCode = await _context.courtsSessions
                .Where(n => n.Id == id)
                .Select(n => n.Code)
                .FirstOrDefaultAsync();

            // If currentCode is 0, you may interpret it as "not found" or invalid.
            if (currentCode == 0)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(direction))
            {
                if (direction.ToLower() == "previous")
                {
                    // Get the record with a code immediately less than the current code.
                    var previousStage = await _context.courtsSessions
                        .Where(n => n.Code < currentCode)
                        .OrderByDescending(n => n.Code)
                        .FirstOrDefaultAsync();
                    return previousStage;
                }
                else if (direction.ToLower() == "next")
                {
                    // Get the record with a code immediately greater than the current code.
                    var nextStage = await _context.courtsSessions
                        .Where(n => n.Code > currentCode)
                        .OrderBy(n => n.Code)
                        .FirstOrDefaultAsync();
                    return nextStage;
                }
            }

            // If no direction was provided or direction is invalid, return null.
            return null;
        }
    }
}
