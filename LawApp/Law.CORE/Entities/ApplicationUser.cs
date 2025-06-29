using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace Law.CORE.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public int UserNumber { get; set; } // رقم المستخدم
        public string Level { get; set; }   // المستوى (مشرف - إضافة - تعديل وإضافة - اظهار)

        // صلاحيات
        public bool CanAccessClient { get; set; }         // مراحل الوكلاء
        public bool CanAccessParty { get; set; }   // ملف الخصوم
        public bool CanAccessCourt { get; set; }   // ملف المحاكم
        public bool CanAccessCenter { get; set; }   // انواع الدوائر 
        public bool CanAccessReceipts { get; set; }   //  سندات قبض 
        public bool CanAccessPayments { get; set; }   // سندات صرف 
        public bool CanAccessPrintReports { get; set; }   // طباعة التقارير  
        public bool CanAccessSearch { get; set; }   // استعلامات  
        public bool CanAccessUsersFile { get; set; }   // ملف المستخدمين  
        public bool CanAccessTypesOfIssue { get; set; }   // انواع القضايا 
    }
}
