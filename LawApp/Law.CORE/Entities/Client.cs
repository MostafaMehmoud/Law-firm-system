using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.Entities
{
    public class Client
    {
        [Key]
        public int Id { get; set; } 
        public int Code {  get; set; }
        public string ClientName { get; set; }  
        public byte[]? ClientImage {  get; set; }
        public string? WorkPhoneNumber {  get; set; }
        public string? FaxNumber { get; set; }
        public string MobileNumber { get; set; }
        public DateOnly IssueDate { get; set; }
        public string? Address { get; set; }
        public string ResponsiblePerson { get; set; }
        public string NationalNumber {  get; set; }
        public string Source { get; set; }
        public DateOnly DateNow { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public decimal LastBalance { get; set; } = 0;
        public string? Email { get; set; }
    }
}
