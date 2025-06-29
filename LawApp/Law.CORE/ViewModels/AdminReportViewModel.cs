using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class AdminReportViewModel
    {
        public int TotalCases { get; set; }
        public int TotalOffers { get; set; }
        public int TotalOpinions { get; set; }
        public int TotalUsers { get; set; }

        public string MostOfferedCaseTitle { get; set; }
        public int MostOfferedCaseCount { get; set; }

        public string TopLawyerName { get; set; }
        public int TopLawyerOfferCount { get; set; }

        public string MostOpinionatedCaseTitle { get; set; }
        public int MostOpinionatedCaseCount { get; set; }
    }

}
