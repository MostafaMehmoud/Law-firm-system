using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Law.CORE.ViewModels
{
    public class UpdateParty
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "الكود مطلوب")]
        public int Code { get; set; }
        [Required(ErrorMessage = "اسم الخصم مطلوب")]
        public string PartyName { get; set; }

        public IFormFile PartyImage { get; set; }
        public string? WorkPhoneNumber { get; set; }
        public string? FaxNumber { get; set; }
        [Required(ErrorMessage = "الجوال مطلوب")]
        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "رجا ادخل تاريخ القضية ")]
        public DateOnly? IssueDate { get; set; }
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
        public string OldImageBase64 { get; set; }
    }
}
