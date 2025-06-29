using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class AddOpinionViewModel
    {
       
            public int Id { get; set; } // ← أضف Id
            public int CaseId { get; set; }

            [Required(ErrorMessage = "يرجى كتابة تعليقك")]
            [MaxLength(500)]
            public string Comment { get; set; }
        }
    

}
