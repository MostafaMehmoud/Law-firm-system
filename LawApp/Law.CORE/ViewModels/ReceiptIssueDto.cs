using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class ReceiptIssueDto
    {
        public int Index { get; set; }
        public int ReceiptNumber { get; set; }
        public string InvoiceOrBond { get; set; }
        public DateOnly Date { get; set; }
        public string IssueTitle { get; set; }
        public decimal DebtorAmount { get; set; }
        public decimal CreditorAmount { get; set; }
        public decimal Balance { get; set; }  // ✅ الرصيد المتغير
        public string Description { get; set; }
    }

}
