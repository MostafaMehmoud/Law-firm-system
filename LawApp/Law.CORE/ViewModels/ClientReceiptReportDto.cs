using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class ClientReceiptReportDto
    {
        public string ClientName { get; set; }
        public List<ReceiptIssueDto> Items { get; set; } = new();
    }

}
