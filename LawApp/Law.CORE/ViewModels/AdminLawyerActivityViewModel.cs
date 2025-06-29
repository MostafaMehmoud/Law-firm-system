using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class AdminLawyerActivityViewModel
    {
        // بيانات المحامي
        public string LawyerName { get; set; }
        public string UserId { get; set; }

        // بيانات القضية
        public string CaseTitle { get; set; }
        public int CaseId { get; set; }

        // بيانات العرض
        public string OfferPhoneNumber { get; set; }
        public string OfferOfficeNumber { get; set; }
        public string ProposedSolution { get; set; }
        public decimal? ProposedFees { get; set; }
        public DateTime? OfferSubmittedAt { get; set; }

        // بيانات الرأي
        public string OpinionComment { get; set; }
        public DateTime? OpinionPostedAt { get; set; }
    }

}
