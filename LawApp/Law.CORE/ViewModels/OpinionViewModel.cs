using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class OpinionViewModel
    {
        public int Id { get; set; }              // 🔴 هذا مهم للزر
        public string UserId { get; set; }
        public string LawyerName { get; set; }
        public string Comment { get; set; }
        public DateTime PostedAt { get; set; }
    }

}
