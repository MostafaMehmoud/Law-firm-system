using Law.CORE.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LawApp.Controllers
{
    public class PermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _permissionName;

        public PermissionAttribute(string permissionName)
        {
            _permissionName = permissionName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;
            var user = httpContext.User;

            // ✅ لو المستخدم مش مسجل دخول
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Users", new { area = "" });
                return;
            }

            // ✅ باقي الفحص للصلاحية
            var userManager = httpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;
            var userTask = userManager.GetUserAsync(user);
            userTask.Wait();
            var appUser = userTask.Result;

            if (appUser == null)
            {
                context.Result = new RedirectToActionResult("Login", "Users", new { area = "" });
                return;
            }

            var property = typeof(ApplicationUser).GetProperty(_permissionName);
            if (property == null || property.PropertyType != typeof(bool))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Users", null);
                return;
            }

            var hasPermission = (bool)property.GetValue(appUser);
            if (!hasPermission)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Users", null);
            }
        }
    }
}
