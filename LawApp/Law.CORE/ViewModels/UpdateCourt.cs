using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class UpdateCourt
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Code { get; set; }
        [Required]
        public string CourtName { get; set; }
        public string? JudgeName { get; set; }

        public string? PhoneNumber { get; set; }
        public string? FaxNumber { get; set; }
    }
}
