using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class CaseViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;

        // معلومات إضافية لحالة العرض والرأي
        public bool HasSubmittedOffer { get; set; }
    }

}
