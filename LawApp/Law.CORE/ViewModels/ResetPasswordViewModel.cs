using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Token { get; set; }

        [Required(ErrorMessage = "كلمة السر الجديدة مطلوبة")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
