using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class FilterDto
    {
        public int? FromClientId { get; set; }
        public int? ToClientId { get; set; }
        public int? FromIssueId { get; set; }
        public int? ToIssueId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
