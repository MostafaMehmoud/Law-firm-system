using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace Law.CORE.ViewModels
{
    public class CreateClient
    {
        [Required(ErrorMessage = "الكود مطلوب")]
        public int Code { get; set; }
        [Required(ErrorMessage = "اسم الوكيل مطلوب")]
        public string ClientName { get; set; }

        public IFormFile ClientImage { get; set; }
        public string? WorkPhoneNumber { get; set; }
        public string? FaxNumber { get; set; }
        [Required(ErrorMessage = "الجوال مطلوب")]
        public string MobileNumber { get; set; }
        public DateOnly IssueDate { get; set; }
        public string? Address { get; set; }
        [Required(ErrorMessage = "الشخص المسؤل مطلوب")]
        public string ResponsiblePerson { get; set; }
        [Required(ErrorMessage = "رقم القومي مطلوب")]
        public string NationalNumber { get; set; }
        [Required(ErrorMessage = "المصدر مطلوب")]
        public string Source { get; set; }
        public DateOnly DateNow { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public decimal LastBalance { get; set; } = 0;
        public string? Email { get; set; }
    }
}
