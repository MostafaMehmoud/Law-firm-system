using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class CaseWithOfferStatusViewModel
    {
        public int CaseId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool HasUserSubmittedOffer { get; set; }
        public int? OfferId { get; set; } // لاستخدامه في زر تعديل
    }

}
