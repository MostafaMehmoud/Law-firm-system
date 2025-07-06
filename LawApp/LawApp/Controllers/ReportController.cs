using Law.BL.Services;
using Law.BL.Services.IServices;
using Law.CORE.ViewModels;
using LawApp.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace LawApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IIssueFileService _issueFileService;
        private readonly IReportService _reportService;
        private readonly IPartyService _partyService;   
        public ReportController(IClientService clientService, IIssueFileService issueFileService,
            IReportService reportService, IPartyService partyService)
        {
            _clientService = clientService;
            _issueFileService = issueFileService;
            _reportService = reportService;
            _partyService = partyService;
        }
        [Permission("CanAccessPrintReports")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Permission("CanAccessPrintReports")]
        public async Task<IActionResult> GetClientsName()
        {
            ViewBag.listIssueFiles = new SelectList((await _issueFileService.GetAll()).ToList(), "Id", "IssueName");
            ViewBag.listClients = new SelectList((await _clientService.GetAll()).ToList(), "Id", "ClientName");
            return View();
        }
        [HttpPost]
        [ApiPermission("CanAccessPrintReports")]
        public async Task<IActionResult> GetClientsReport([FromBody] FilterDto filter)
        {
            var result = await _reportService.GetClientsReportViewModel(filter);
            return Json(result);
        }
        [HttpGet]
        [Permission("CanAccessPrintReports")]
        public async Task<IActionResult> GetPartysName()
        {
            ViewBag.listIssueFiles = new SelectList((await _issueFileService.GetAll()).ToList(), "Id", "IssueName");
            ViewBag.listPartys = new SelectList((await _partyService.GetAll()).ToList(), "Id", "PartyName");
            return View();
        }
        [HttpPost]
        [ApiPermission("CanAccessPrintReports")]
        public async Task<IActionResult> GetPartysReport([FromBody] FilterDto filter)
        {
            var result = await _reportService.GetPartysReportViewModel(filter);
            return Json(result);
        }
        [HttpGet]
        [Permission("CanAccessPrintReports")]
        public async Task<IActionResult> GetCourtsName()
        {
             return View();
        }
        [HttpPost]
        [ApiPermission("CanAccessPrintReports")]
        public async Task<IActionResult> GetCourtsReport()
        {
            var result = await _reportService.GetCourtsReportViewModel();
            return Json(result);
        }
        [HttpGet]
        [Permission("CanAccessPrintReports")] 
        public async Task<IActionResult> GetCentersName()
        {
            return View();
        }
        [HttpPost]
        [ApiPermission("CanAccessPrintReports")]
        public async Task<IActionResult> GetCentersReport()
        {
            var result = await _reportService.GetCentersReportViewModel();
            return Json(result);
        }
        [Permission("CanAccessSearch")]
        public async Task<IActionResult> GetIssuesName()
        {
            ViewBag.listIssueFiles = new SelectList((await _issueFileService.GetAll()).ToList(), "Id", "IssueName");
            ViewBag.listClients = new SelectList((await _clientService.GetAll()).ToList(), "Id", "ClientName");
            return View();
        }
        [HttpPost]
        [ApiPermission("CanAccessSearch")]
        public async Task<IActionResult> GetIssuesReport([FromBody] FilterDto filter)
        {
            var result = await _reportService.GetIssuesReportAsync(filter);
            return Json(result);
        }
        [Permission("CanAccessSearch")]
        public async Task<IActionResult> GetReceiptIssue()
        {
            ViewBag.listIssueFiles = new SelectList((await _issueFileService.GetAll()).ToList(), "Id", "IssueName");
            ViewBag.listClients = new SelectList((await _clientService.GetAll()).ToList(), "Id", "ClientName");
            return View();
        }
        [HttpPost]
        [ApiPermission("CanAccessSearch")]
        public async Task<IActionResult> GetReceiptIssue([FromBody] FilterDto filter)
        {
            var result = await _reportService.GetIssuesReportAsync(filter);
            return Json(result);
        }
        [Permission("CanAccessSearch")]
        public async Task<IActionResult> GetAgentAccountStatement()
        {
            ViewBag.listIssueFiles = new SelectList((await _issueFileService.GetAll()).ToList(), "Id", "IssueName");
            ViewBag.listClients = new SelectList((await _clientService.GetAll()).ToList(), "Id", "ClientName");
            return View();
        }
        [HttpPost]
        [ApiPermission("CanAccessSearch")]
        public async Task<IActionResult> GetAgentAccountStatement(FilterDto filter)
        {
            

            var result = await _reportService.GetReceiptIssueGroupedByClient(filter);
            return Json(result);
        }
        [Permission("CanAccessSearch")]
        public async Task<IActionResult> AdminDashboard()
        {
            var model = await _reportService.GetAdminReportAsync();
            return View(model);
        }
        [Permission("CanAccessSearch")]
        public async Task<IActionResult> LawyerActivity()
        {
            var model = await _reportService.GetAllLawyerActivityAsync();
            return View(model);
        }

    }
}
