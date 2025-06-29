using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.Entities
{
    public class Opinion
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CaseId { get; set; }
        public string Comment { get; set; }
        public DateTime PostedAt { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Case Case { get; set; }
    }

}
