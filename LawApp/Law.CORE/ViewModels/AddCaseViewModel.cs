using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class AddCaseViewModel
    {
        [Required(ErrorMessage = "يرجى إدخال عنوان القضية")]
        public string Title { get; set; }

        [Required(ErrorMessage = "يرجى إدخال وصف القضية")]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
