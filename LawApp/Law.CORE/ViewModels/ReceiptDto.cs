using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class ReceiptDto
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public DateOnly DateReceipt { get; set; }
        public int ClientId { get; set; }
        public decimal Amount { get; set; }
        public string AmountName { get; set; }
        public int PaymentId { get; set; }
        public int? ChequeNumber { get; set; }
        public DateOnly? ChequeDate { get; set; }
        public string? Purpose { get; set; }
        public string? BankName { get; set; }
        public int DesignType { get; set; }
        public int? DesignNumber { get; set; }
        public int? ProjectNumber { get; set; }
        public string? ProjectName { get; set; }
    }
}
