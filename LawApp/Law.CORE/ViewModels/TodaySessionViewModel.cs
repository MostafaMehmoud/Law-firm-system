using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class TodaySessionViewModel
    {
        public string IssueName { get; set; }              // اسم القضية
        public string ClientName { get; set; }             // اسم الموكل
        public string ClientPhone { get; set; }            // الهاتف
        public string PartyName { get; set; }              // اسم الخصم (من PartyAddress أو PartyWorkPlace)
        public string CourtName { get; set; }                   // رقم المحكمة
        public string CenterName { get; set; }                  // رقم الدائرة
        public string ClaimNumber { get; set; }            // رقم القضية
        public string Notes { get; set; }                  // ملاحظات الجلسة
    }

}
