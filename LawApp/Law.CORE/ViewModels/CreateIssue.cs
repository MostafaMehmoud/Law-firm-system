using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class CreateIssue
    {
        [Required]
        public int Code { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
