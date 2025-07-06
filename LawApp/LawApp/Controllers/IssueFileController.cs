using Law.BL.Services.IServices;
using Law.CORE.ViewModels;
using LawApp.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LawApp.Controllers
{
    public class IssueFileController : Controller
    {
        private readonly ICourtService _courtService;
        private readonly IIssueService _issueService;   
        private readonly ICenterService _centerService;
        private readonly IClientService _clientService;
        private readonly IPartyService _partyService;
        private readonly IIssueFileService _issueFileService;
        public IssueFileController(ICourtService courtService,IIssueService issueService,
            ICenterService centerService,IClientService clientService
            , IPartyService partyService, IIssueFileService issueFileService
            )
        {
            _centerService = centerService;
            _courtService = courtService;
            _issueService= issueService;    
            _clientService = clientService;
            _partyService = partyService;   
            _issueFileService = issueFileService;
        }
        [Permission("CanAccessTypesOfIssue")]
        public async Task<IActionResult> Index()
        {
          
            ViewBag.listCenters = new SelectList((await _centerService.GetAll()).ToList(), "Id", "Name");
            ViewBag.listCourts = new SelectList((await _courtService.GetAll()).ToList(), "Id", "CourtName");
            ViewBag.listIssueTypies = new SelectList((await _issueService.GetAll()).ToList(), "Id", "Name");
            ViewBag.listClients = new SelectList((await _clientService.GetAll()).ToList(), "Id", "ClientName");
            ViewBag.listParties = new SelectList((await _partyService.GetAll()).ToList(), "Id", "PartyName");
            return View();
        }
        [ApiPermission("CanAccessTypesOfIssue")]
        public async Task<IActionResult> GetNextIssueFileCode()
        {
            try
            {
                var nextCode = await _issueFileService.GetNewCode();
                return Json(new { nextCode });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Exception in GetNextIssueFileCode: " + ex.ToString());
                // Return full details for debugging (remove before production)
                return StatusCode(500, new { error = ex.Message, details = ex.ToString() });
            }
        }
        [HttpPost]
        [ApiPermission("CanAccessTypesOfIssue")]
        public async Task<IActionResult> AddIssueFile([FromForm] UpdateIssueFile model)
        {
           
            ModelState.Remove("Id");
            ModelState.Remove("DateNow");
            ModelState.Remove("OldImageBase64");
            if (!ModelState.IsValid)
            {
                // Collect validation errors
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                return BadRequest(errors);
            }

            // Use your BL service to add the record
            var resultMessage = await _issueFileService.Add(model);
            // For this example, assume the service returns a success message string
            bool success = resultMessage.Contains("نجاح");

            return Ok(new { success, message = resultMessage });
        }
        [HttpPost]
        [ApiPermission("CanAccessTypesOfIssue")]
        public async Task<IActionResult> EditIssueFile([FromForm] UpdateIssueFile model)
        {
            ModelState.Remove("DateNow");
            ModelState.Remove("OldImageBase64");
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


            var resultMessage = await _issueFileService.Edit(model);
            bool success = resultMessage.Contains("نجاح");

            return Ok(new { success, message = resultMessage });
        }
        [HttpPost]
        [ApiPermission("CanAccessTypesOfIssue")]
        public async Task<IActionResult> DeleteIssueFile(int id)
        {
            try
            {
                string result = await _issueFileService.Delete(id);
                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("GetMinIssueFile")]
        [ApiPermission("CanAccessTypesOfIssue")]
        public async Task<IActionResult> GetMinIssueFile()
        {
            var IssueFile = await _issueFileService.GetMinIssueFile();
            if (IssueFile == null)
                return NotFound(new { Message = "No records found." });
            return Ok(IssueFile);
        }

        [HttpGet("GetMaxIssueFile")]
        [ApiPermission("CanAccessTypesOfIssue")]
        public async Task<IActionResult> GetMaxIssueFile()
        {
            var IssueFile = await _issueFileService.GetMaxIssueFile();
            if (IssueFile == null)
                return NotFound(new { Message = "No records found." });
            return Ok(IssueFile);
        }

        [HttpGet("GetNextIssueFile/{id}")]
        [ApiPermission("CanAccessTypesOfIssue")]
        public async Task<IActionResult> GetNextIssueFile(int id)
        {
            if (id == 0)
            {
                id = _issueFileService.GetMaxIssueFileId();
            }
            var IssueFile = await _issueFileService.GetNextIssueFile(id);
            if (IssueFile == null)
                return NotFound(new { Message = "No next record found." });
            return Ok(IssueFile);
        }

        [HttpGet("GetPreviousIssueFile/{id}")]
        [ApiPermission("CanAccessTypesOfIssue")]
        public async Task<IActionResult> GetPreviousIssueFile(int id)
        {
            if (id == 0)
            {
                id = _issueFileService.GetMaxIssueFileId();
            }
            var IssueFile = await _issueFileService.GetPreviousIssueFile(id);
            if (IssueFile == null)
                return NotFound(new { Message = "No previous record found." });
            return Ok(IssueFile);
        }
    }
}
