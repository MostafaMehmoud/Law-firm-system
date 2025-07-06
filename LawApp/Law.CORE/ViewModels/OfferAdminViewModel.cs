using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class OfferAdminViewModel
    {
        public int OfferId { get; set; }
        public string LawyerName { get; set; }
        public string PhoneNumber { get; set; }
        public string OfficeNumber { get; set; }
        public string ProposedSolution { get; set; }
        public decimal ProposedFees { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string CaseTitle { get; set; }
        public int CaseId { get; set; }
        public bool IsAwarded { get; set; }  // هل تم ترسية العرض؟
    }

}
