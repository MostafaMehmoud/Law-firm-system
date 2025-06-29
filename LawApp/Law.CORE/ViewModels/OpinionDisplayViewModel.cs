using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class OpinionDisplayViewModel
    {
        public int Id {  get; set; }
        public string CaseTitle { get; set; }
        public string LawyerName { get; set; }
        public string Comment { get; set; }
        public DateTime PostedAt { get; set; }
    }

}
