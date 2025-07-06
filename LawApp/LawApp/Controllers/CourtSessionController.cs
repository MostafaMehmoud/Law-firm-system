using Law.BL.Services.IServices;
using Law.CORE.Entities;
using Law.CORE.ViewModels;
using LawApp.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LawApp.Controllers
{
    public class CourtSessionController : Controller
    {
        private readonly ICourtService _courtService;
        private readonly IIssueService _issueService;
        private readonly ICenterService _centerService;
        private readonly IClientService _clientService;
        private readonly IPartyService _partyService;
        private readonly IIssueFileService _issueFileService;
        private readonly ICourtSessionService _courtSessionService;
        public CourtSessionController(ICourtService courtService, IIssueService issueService,
            ICenterService centerService, IClientService clientService
            , IPartyService partyService, IIssueFileService issueFileService,
            ICourtSessionService courtSessionService
            )
        {
            _centerService = centerService;
            _courtService = courtService;
            _issueService = issueService;
            _clientService = clientService;
            _partyService = partyService;
            _issueFileService = issueFileService;
            _courtSessionService = courtSessionService;
        }
        [Permission("CanAccessPayments")]
        public async Task<IActionResult> Index()
        {
            ViewBag.listIssueFiles= new SelectList((await _issueFileService.GetAll()).ToList(), "Id", "IssueName");
            ViewBag.listCenters = new SelectList((await _centerService.GetAll()).ToList(), "Id", "Name");
            ViewBag.listCourts = new SelectList((await _courtService.GetAll()).ToList(), "Id", "CourtName");
            ViewBag.listIssueTypies = new SelectList((await _issueService.GetAll()).ToList(), "Id", "Name");
            ViewBag.listClients = new SelectList((await _clientService.GetAll()).ToList(), "Id", "ClientName");
            ViewBag.listParties = new SelectList((await _partyService.GetAll()).ToList(), "Id", "PartyName");
            return View();
        }
        [ApiPermission("CanAccessTypesOfIssue")]
        public async Task<IActionResult> GetIssueDetails(int id)
        {
            IssueFile result =await _issueFileService.GetbyId(id);
            return Json(result);
        }
        [ApiPermission("CanAccessTypesOfIssue")]
        public async Task<IActionResult> GetNextCourtSessionCode()
        {
            try
            {
                var nextCode = await _courtSessionService.GetNewCode();
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
        public async Task<IActionResult> AddCourtSession([FromForm] UpdateCourtSession model)
        {

            ModelState.Remove("Id");
            ModelState.Remove("CourtSessionDate");
            ModelState.Remove("IssueImage");
            
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
            var resultMessage = await _courtSessionService.Add(model);
            // For this example, assume the service returns a success message string
            bool success = resultMessage.Contains("نجاح");

            return Ok(new { success, message = resultMessage });
        }
        [HttpPost]
        [ApiPermission("CanAccessTypesOfIssue")]
        public async Task<IActionResult> EditCourtSession([FromForm] UpdateCourtSession model)
        {
            ModelState.Remove("CourtSessionDate");
            ModelState.Remove("IssueImage");

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


            var resultMessage = await _courtSessionService.Edit(model);
            bool success = resultMessage.Contains("نجاح");

            return Ok(new { success, message = resultMessage });
        }
        [HttpPost]
        [ApiPermission("CanAccessTypesOfIssue")]
        public async Task<IActionResult> DeleteCourtSession(int id)
        {
            try
            {
                string result = await _courtSessionService.Delete(id);
                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("GetMinCourtSession")]
        [ApiPermission("CanAccessTypesOfIssue")]
        public async Task<IActionResult> GetMinCourtSession()
        {
            CourtSessionDto CourtSession = await _courtSessionService.GetMinCourtSession();
            if (CourtSession == null)
                return NotFound(new { Message = "No records found." });
            return Ok(CourtSession);
        }

        [HttpGet("GetMaxCourtSession")]
        [ApiPermission("CanAccessTypesOfIssue")]
        public async Task<IActionResult> GetMaxCourtSession()
        {
            var CourtSession = await _courtSessionService.GetMaxCourtSession();
            if (CourtSession == null)
                return NotFound(new { Message = "No records found." });
            return Ok(CourtSession);
        }

        [HttpGet("GetNextCourtSession/{id}")]
        [ApiPermission("CanAccessTypesOfIssue")]
        public async Task<IActionResult> GetNextCourtSession(int id)
        {
            if (id == 0)
            {
                id = _courtSessionService.GetMaxCourtSessionId();
            }
            var CourtSession = await _courtSessionService.GetNextCourtSession(id);
            if (CourtSession == null)
                return NotFound(new { Message = "No next record found." });
            return Ok(CourtSession);
        }

        [HttpGet("GetPreviousCourtSession/{id}")]
        [ApiPermission("CanAccessTypesOfIssue")]
        public async Task<IActionResult> GetPreviousCourtSession(int id)
        {
            if (id == 0)
            {
                id = _courtSessionService.GetMaxCourtSessionId();
            }
            var CourtSession = await _courtSessionService.GetPreviousCourtSession(id);
            if (CourtSession == null)
                return NotFound(new { Message = "No previous record found." });
            return Ok(CourtSession);
        }
    }
}
