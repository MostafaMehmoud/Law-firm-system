﻿using System.Security.Claims;
using Law.BL.Services;
using Law.BL.Services.IServices;
using Law.CORE.Entities;
using Law.CORE.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LawApp.Controllers
{
    public class CaseController : Controller
    {
        private readonly IOpinionService _opinionService;
        private readonly ICaseService _caseService;
        private readonly IAuthService _authService;

        public CaseController(ICaseService caseService, IAuthService authService, IOpinionService opinionService)
        {
            _caseService = caseService;
            _authService = authService;
            _opinionService = opinionService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _authService.GetUserByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null)
            {
                return RedirectToAction("Login", "Users");
            }
            var model = await _caseService.GetCasesWithOfferStatusAsync(user.Id);
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var user = await _authService.GetUserByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var caseDetails = await _caseService.GetCaseByIdAsync(id);
            if (caseDetails == null)
                return NotFound();

            var opinions = await _opinionService.GetOpinionsByCaseIdAsync(id);

            var viewModel = new CaseDetailViewModel
            {
              
                Opinions = opinions.Select(o=>new OpinionViewModel
                {
                    Id=o.Id,
                    LawyerName=o.LawyerName,
                    Comment=o.Comment,
                    PostedAt=o.PostedAt,
                    UserId= user.Id,

                }).ToList()
            };

            return View(viewModel);
        }
        [HttpGet]
        [Permission("CanAccessUsersFile")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Permission("CanAccessUsersFile")]
        public async Task<IActionResult> Add(CaseViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _caseService.AddCaseAsync(model);
            return RedirectToAction("Index", "Case");
        }
        public async Task<IActionResult> AllCases()
        {
            var user = await _authService.GetUserByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if(user == null)
            {
                return RedirectToAction("Login", "Users");
            }
            var model = await _caseService.GetCasesWithOfferStatusForUserAsync(user.Id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("CanAccessUsersFile")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _caseService.DeleteCaseAsync(id);

            if (result)
            {
                TempData["Success"] = "تم حذف القضية بنجاح.";
            }
            else
            {
                TempData["Error"] = "حدث خطأ أثناء حذف القضية.";
            }

            return RedirectToAction("Index");
        }

    }

}
