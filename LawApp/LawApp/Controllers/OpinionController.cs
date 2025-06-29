using Law.BL.Services.IServices;
using Law.CORE.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Law.CORE.Entities;
using Microsoft.AspNetCore.Identity;
using Law.DAL.Repository.IRepository;

namespace LawApp.Controllers
{
    public class OpinionController : Controller
    {
        private readonly IOpinionService _opinionService;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        public OpinionController(IOpinionService opinionService, IAuthService authService
            ,IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _opinionService = opinionService;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult AddOpinion(int caseId)
        {
            var model = new AddOpinionViewModel { CaseId = caseId };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOpinion(AddOpinionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId  = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // ❌ تحقق من تكرار الرأي
            bool hasOpinion = await _opinionService.HasUserSubmittedOpinionAsync(userId, model.CaseId);
            if (hasOpinion)
            {
                TempData["Error"] = "لقد قمت بإرسال رأي مسبقًا على هذه القضية.";
                return RedirectToAction("Index", "Case");
            }

            // ✅ إنشاء الرأي
            var opinion = new Opinion
            {
                CaseId = model.CaseId,
                UserId = userId,
                Comment = model.Comment,
                PostedAt = DateTime.Now
            };

            await _unitOfWork.OpinionRepository.Add(opinion);
             _unitOfWork.Complete();

            TempData["Success"] = "تم إرسال رأيك بنجاح";
            return RedirectToAction("Index", "Case");
        }

        public async Task<IActionResult> MyOpinions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await _opinionService.GetUserOpinionsAsync(userId);
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditOpinion(int id)
        {
            var model = await _opinionService.GetForEditAsync(id);
            if (model == null)
                return NotFound();

            var userId  = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var opinion = await _unitOfWork.OpinionRepository.GetByIdAsync(id);

            if (opinion.UserId != userId)
                return Forbid();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOpinion(OpinionEditViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updated = await _opinionService.UpdateOpinionAsync(model, userId);

            if (!updated)
            {
                TempData["Error"] = "حدث خطأ. لا يمكنك تعديل هذا الرأي.";
                return RedirectToAction("Details", "Case", new { id = model.CaseId });
            }

            TempData["Success"] = "تم تعديل رأيك بنجاح.";
            return RedirectToAction("Details", "Case", new { id = model.CaseId });
        }

        [HttpPost]
        //public async Task<IActionResult> DeleteOpinion(int id)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var success = await _opinionService.DeleteOpinionAsync(id, userId);
        //    return RedirectToAction("MyOpinions");
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOpinion(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var opinion = await _unitOfWork.OpinionRepository.GetByIdAsync(id);

            if (opinion == null || opinion.UserId != userId)
            {
                TempData["Error"] = "لا يمكنك حذف هذا الرأي.";
                return RedirectToAction("Index", "Case");
            }

            await _unitOfWork.OpinionRepository.Delete(opinion.Id);
             _unitOfWork.Complete();

            TempData["Success"] = "تم حذف الرأي بنجاح.";
            return RedirectToAction("Details", "Case", new { id = opinion.CaseId });
        }

    }

}
