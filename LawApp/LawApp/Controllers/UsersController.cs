using Law.CORE.ViewModels;
using System.Security.Permissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Law.BL.Services.IServices;

namespace LawApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAuthService _iserviceAuth;
        public UsersController(IAuthService iserviceAuth)
        {
            _iserviceAuth = iserviceAuth;
        }
        public List<SelectListItemLevel> GetLevel()
        {
            List<SelectListItemLevel> Levels = new List<SelectListItemLevel>()
            {
                new SelectListItemLevel { Text = "مشرف", Value = UserLevels.Admin },
            new SelectListItemLevel { Text = "إضافة", Value = UserLevels.Add },
            new SelectListItemLevel { Text = "تعديل وإضافة", Value = UserLevels.EditAdd },
            new SelectListItemLevel { Text = "إظهار", Value = UserLevels.View }
            };
            return Levels;
        }
        [Permission("CanAccessUsersFile")]
        public IActionResult Create()
        {
            var Model = new VWUser();
            ViewBag.ListLevels = new SelectList(GetLevel(), "Value", "Text");
            return View(Model);
        }
        public async Task<IActionResult> GetNextCode()
        {
            try
            {
                var nextCode = await _iserviceAuth.GetNewCode();
                return Json(new { nextCode });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Exception in GetNextNationalCode: " + ex.ToString());
                // Return full details for debugging (remove before production)
                return StatusCode(500, new { error = ex.Message, details = ex.ToString() });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add(VWUser user)
        {
            ModelState.Remove("Id"); // في حالة استخدام تسجيل جديد
            ModelState.Remove("Levels"); // في حالة استخدام تسجيل جديد

            if (!ModelState.IsValid)
            {
                // جمع أخطاء الـ validation
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                return BadRequest(errors);
            }

            // الاتصال بالخدمة
            var resultMessage = await _iserviceAuth.RegisterAsync(user);

            if (resultMessage.Success)
            {
                return Ok(new { success = true, message = resultMessage.Message });
            }

            // في حالة وجود خطأ من RegisterAsync
            return BadRequest(new { success = false, errors = resultMessage.Errors });

        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {

            var result = await _iserviceAuth.DeleteUserAsync(id);
            if (result.Success)
            {
                return Json(new { success = true, message = result.Message });
            }
            return BadRequest(new { success = false, errors = result.Errors });

        }
        [HttpPost]
        public async Task<IActionResult> Edit(VWUser user)
        {


            ModelState.Remove("Levels");
            ModelState.Remove("Password");


            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                return BadRequest(errors);
            }

            var resultMessage = await _iserviceAuth.UpdateUserAsync(user);
            if (resultMessage.Success)
            {
                return Ok(new { success = true, message = resultMessage.Message });
            }

            // في حالة وجود خطأ من RegisterAsync
            return BadRequest(new { success = false, errors = resultMessage.Errors });
        }
        [HttpGet("GetMin")]
        public async Task<IActionResult> GetMin()
        {
            var ClassType = await _iserviceAuth.GetMin();
            if (ClassType == null)
                return NotFound(new { Message = "No records found." });
            return Ok(ClassType);
        }

        [HttpGet("GetMax")]
        public async Task<IActionResult> GetMax()
        {
            var national = await _iserviceAuth.GetMax();
            if (national == null)
                return NotFound(new { Message = "No records found." });
            return Ok(national);
        }

        [HttpGet("GetNext/{id}")]
        public async Task<IActionResult> GetNext(int id)
        {
            if (id == 0)
            {
                id = await _iserviceAuth.GetMaxIdOfItem();
            }
            var national = await _iserviceAuth.GetNextUser(id);
            if (national == null)
                return NotFound(new { Message = "No next record found." });
            return Ok(national);
        }

        [HttpGet("GetPrevious/{id}")]
        public async Task<IActionResult> GetPrevious(int id = 0)
        {




            if (id == 0)
            {
                id = await _iserviceAuth.GetMaxIdOfItem();
            }
            var national = await _iserviceAuth.GetPreviousUser(id);
            if (national == null)
                return NotFound(new { Message = "No next record found." });
            return Ok(national);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _iserviceAuth.LoginAsync(model.Username, model.Password);
                if (result.Success)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Errors[0]);
                }
            }

            return View(model); // This will send the ModelState back to the view
        }
        [HttpPost]

        public async Task<IActionResult> Logout()
        {
            _iserviceAuth.Logout();
            return Json(new { success = true });  // إرسال استجابة تفيد بأن عملية تسجيل الخروج تمت بنجاح
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordByUsernameViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var resetLink = await _iserviceAuth.GenerateResetPasswordLinkAsync(model.Username, Request, Url);

            if (resetLink == null)
            {
                TempData["ErrorMessage"] = "اسم المستخدم غير موجود.";
                return View(model);
            }

            // ✅ التحويل مباشرة إلى صفحة إعادة التعيين
            return Redirect(resetLink);
        }


        [HttpGet]
        public IActionResult ResetPassword(string token, string username)
        {
            return View(new ResetPasswordViewModel { Token = token, Username = username });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _iserviceAuth.ResetPasswordAsync(model.Username, model.Token, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "تمت إعادة تعيين كلمة السر بنجاح.";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("NewPassword", error.Description);

            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(VWUser model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ModelState.Remove("Id"); // في حالة استخدام تسجيل جديد
            ModelState.Remove("Levels"); // في حالة استخدام تسجيل جديد
            if (ModelState.IsValid)
            {
                var result = await _iserviceAuth.RegisterWithoutRolesAsync(model);
                if (result.Success)
                {
                    return LocalRedirect("/Home/Index");
                }
                else
                {
                    ModelState.AddModelError("Password", result.Errors[0]);
                }
            }

            return View(model); // This will send the ModelState back to the view
        }
    }
}
