using Law.CORE.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LawApp.Attributes
{
    public class ApiPermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _permissionName;

        public ApiPermissionAttribute(string permissionName)
        {
            _permissionName = permissionName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;
            var user = httpContext.User;

            // ✅ تحقق إن المستخدم مسجل دخول
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult(); // 401
                return;
            }

            // ✅ جلب المستخدم
            var userManager = httpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;
            var userTask = userManager.GetUserAsync(user);
            userTask.Wait();
            var appUser = userTask.Result;

            if (appUser == null)
            {
                context.Result = new UnauthorizedResult(); // 401
                return;
            }

            // ✅ التحقق من وجود الخاصية
            var property = typeof(ApplicationUser).GetProperty(_permissionName);
            if (property == null || property.PropertyType != typeof(bool))
            {
                context.Result = new ForbidResult(); // 403
                return;
            }

            // ✅ التحقق من صلاحية المستخدم
            var hasPermission = (bool)property.GetValue(appUser);
            if (!hasPermission)
            {
                context.Result = new ForbidResult(); // 403
            }
        }
    }
}
