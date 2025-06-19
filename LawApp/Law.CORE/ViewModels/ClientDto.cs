using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class ClientDto
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string ClientName { get; set; }
        public string? ClientImageBase64 { get; set; }
        public string? WorkPhoneNumber { get; set; }
        public string? FaxNumber { get; set; }
        public string MobileNumber { get; set; }
        public DateOnly IssueDate { get; set; }
        public string? Address { get; set; }
        public string ResponsiblePerson { get; set; }
        public string NationalNumber { get; set; }
        public string Source { get; set; }
        public DateOnly DateNow { get; set; }
        public decimal LastBalance { get; set; }
        public string? Email { get; set; }
      
    }
}
