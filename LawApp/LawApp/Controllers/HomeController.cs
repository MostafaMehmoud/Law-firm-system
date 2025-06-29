using System.Diagnostics;
using Law.BL.Services;
using Law.BL.Services.IServices;
using LawApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LawApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOpinionService _opinionService;
        private readonly IReportService _reportService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,IOpinionService opinionService
            ,IReportService reportService)
        {
            _reportService = reportService;
            _opinionService = opinionService;   
            _logger = logger;
        }
        public async Task<IActionResult> TodaySessions()
        {
            var model = await _reportService.GetTodaySessionsViewModelAsync();
            return View(model);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Permission("CanAccessUsersFile")]
        public async Task<IActionResult> Opinions()
        {
            var opinions = await _opinionService.GetAllOpinionsForAdminAsync();
            return View(opinions);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
