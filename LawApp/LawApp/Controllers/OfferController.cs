using Law.BL.Services.IServices;
using Law.CORE.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Law.BL.Services;

namespace LawApp.Controllers
{
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly IAuthService _authService;
        private readonly IReportService _reportService; 

        public OfferController(IOfferService offerService, IAuthService authService, IReportService reportService)
        {
            _offerService = offerService;
            _authService = authService;
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> SubmitOffer(int caseId)
        {
            var user = await _authService.GetUserByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var hasSubmitted = await _offerService.HasUserSubmittedOfferAsync(user.Id, caseId);
            if (hasSubmitted)
                return RedirectToAction("Index", "Case");

            var model = new SubmitOfferViewModel { CaseId = caseId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitOffer(SubmitOfferViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _authService.GetUserByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var success = await _offerService.SubmitOfferAsync(model, user.Id);

            if (!success)
            {
                ModelState.AddModelError("", "لقد قمت بتقديم عرض مسبقًا لهذه القضية.");
                return View(model);
            }

            return RedirectToAction("Index", "Case");
        }
        [HttpGet]
        public async Task<IActionResult> EditOffer(int caseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await _offerService.GetOfferForEditAsync(userId, caseId);
            if (model == null)
                return RedirectToAction("Index", "Case");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditOffer(SubmitOfferViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _offerService.UpdateOfferAsync(model, userId);

            if (!success)
            {
                ModelState.AddModelError("", "فشل في تعديل العرض. تأكد من أنك صاحب العرض.");
                return View(model);
            }

            return RedirectToAction("Index", "Case");
        }
        public async Task<IActionResult> MyOffers()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await _offerService.GetOffersByUserAsync(userId);
            return View(model);
        }
        public async Task<IActionResult> AllOffers()
        {
            var model = await _reportService.GetAllOffersForAdminAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AwardOffer(int offerId)
        {
            var success = await _reportService.AwardOfferAsync(offerId);
            TempData["Success"] = success ? "تم ترسية العرض بنجاح." : "فشل في ترسية العرض.";
            return RedirectToAction("AllOffers");
        }

    }

}
