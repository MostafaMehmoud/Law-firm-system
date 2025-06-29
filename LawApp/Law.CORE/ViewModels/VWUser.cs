using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class VWUser

    {
        public string Id { get; set; }
    public int UserNumber { get; set; }

    public string Password { get; set; }
        public string Level { get; set; } = UserLevels.View;
    public string UserName { get; set; }
    public string? Email { get; set; }
    public List<SelectListItemLevel> Levels { get; set; }
        // صلاحيات
        public bool CanAccessClient { get; set; } = false;         // مراحل الوكلاء
        public bool CanAccessParty { get; set; } = false;    // ملف الخصوم
        public bool CanAccessCourt { get; set; } = false;    // ملف المحاكم
        public bool CanAccessCenter { get; set; } = false;   // انواع الدوائر 
        public bool CanAccessTypesOfIssue { get; set; } = false;   // انواع القضايا 
        public bool CanAccessReceipts { get; set; } = false;    //  سندات قبض 
        public bool CanAccessPayments { get; set; } = false;   // سندات صرف 
        public bool CanAccessPrintReports { get; set; } = false;    // طباعة التقارير  
        public bool CanAccessSearch { get; set; } = false;   // استعلامات  
        public bool CanAccessUsersFile { get; set; } = false;   // ملف المستخدمين  
    }
    public class SelectListItemLevel
{
    public string Text { get; set; }
    public string Value { get; set; }
}
}
