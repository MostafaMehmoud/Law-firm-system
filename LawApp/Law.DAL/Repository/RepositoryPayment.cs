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
    public class RepositoryPayment : RepositoryBase<Payment>, IRepositorySpecial<Payment>
    {
        public RepositoryPayment(LawAppDbContext context) : base(context)
        {
        }

        public async Task<Payment> GetMax()
        {
            var maxStage = await _context.payments.OrderByDescending(n => n.Code).FirstOrDefaultAsync();
            return maxStage;
        }

        public int GetMaxIdOfItem()
        {
            var id = _context.payments.Max(i => i.Id);
            return id;
        }

        public async Task<Payment> GetMin()
        {
            var minSatge = await _context.payments.OrderBy(n => n.Code).FirstOrDefaultAsync();
            return minSatge;
        }

        public async Task<int> GetNewCode()
        {
            try
            {
                // Use DefaultIfEmpty(0) to handle an empty table.
                int? maxCode = await _context.payments
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

        public async Task<Payment> GetNextOrPreviousItemByCode(int id, string direction)
        {
            // Get the current record's code.
            var currentCode = await _context.payments
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
                    var previousStage = await _context.payments
                        .Where(n => n.Code < currentCode)
                        .OrderByDescending(n => n.Code)
                        .FirstOrDefaultAsync();
                    return previousStage;
                }
                else if (direction.ToLower() == "next")
                {
                    // Get the record with a code immediately greater than the current code.
                    var nextStage = await _context.payments
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
