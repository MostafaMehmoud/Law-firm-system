using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;

namespace Law.CORE.ViewModels
{
    public class CaseDetailsViewModel
    {
        public Case Case { get; set; }
        public List<OpinionDisplayViewModel> Opinions { get; set; }
    }

}
