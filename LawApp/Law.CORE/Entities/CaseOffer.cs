using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.Entities
{
    public class Offer
    {
        public int Id { get; set; }
        public string UserId { get; set; }           // FK to Identity User
        public int CaseId { get; set; }              // FK to Case
        public string LawyerName { get; set; }
        public string PhoneNumber { get; set; }
        public string OfficeNumber { get; set; }
        public string ProposedSolution { get; set; }
        public decimal ProposedFees { get; set; }
        public DateTime SubmittedAt { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Case Case { get; set; }
    }

}
