using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class IssueReportDto
    {
        public int Index { get; set; }
        public string IssueNumber { get; set; }
        public string IssueDate { get; set; } // أو DateTime
        public string IssueTitle { get; set; }
        public string ClientName { get; set; }
        public string PartyName { get; set; }
        public string CourtName { get; set; }
        public string CircleName { get; set; }
        public string JudgeName { get; set; }
        public decimal IssueValueFees { get; set; } = 0;
    }
}
