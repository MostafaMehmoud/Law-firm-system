using Law.BL.Services.IServices;
using Law.CORE.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LawApp.Controllers
{
    public class IssueController : Controller
    {
        private readonly IIssueService _service;
        public IssueController(IIssueService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetNextIssueCode()
        {
            try
            {
                var nextCode = await _service.GetNewCode();
                return Json(new { nextCode });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Exception in GetNextIssueCode: " + ex.ToString());
                // Return full details for debugging (remove before production)
                return StatusCode(500, new { error = ex.Message, details = ex.ToString() });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddIssue([FromBody] CreateIssue model)
        {
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
            var resultMessage =await _service.Add(model);
            // For this example, assume the service returns a success message string
            bool success = resultMessage.Contains("نجاح");

            return Ok(new { success, message = resultMessage });
        }
        [HttpPost]
        public async Task<IActionResult> EditIssue([FromBody] UpdateIssue model)
        {
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

            var resultMessage =await _service.Edit(model);
            bool success = resultMessage.Contains("نجاح");

            return Ok(new { success, message = resultMessage });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteIssue(int id)
        {
            try
            {
                string result =await _service.Delete(id);
                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("GetMinIssue")]
        public async Task<IActionResult> GetMinIssue()
        {
            var Issue = await _service.GetMinIssue();
            if (Issue == null)
                return NotFound(new { Message = "No records found." });
            return Ok(Issue);
        }

        [HttpGet("GetMaxIssue")]
        public async Task<IActionResult> GetMaxIssue()
        {
            var Issue = await _service.GetMaxIssue();
            if (Issue == null)
                return NotFound(new { Message = "No records found." });
            return Ok(Issue);
        }

        [HttpGet("GetNextIssue/{id}")]
        public async Task<IActionResult> GetNextIssue(int id)
        {
            if (id == 0)
            {
                id = _service.GetMaxIssueId();
            }
            var Issue = await _service.GetNextIssue(id);
            if (Issue == null)
                return NotFound(new { Message = "No next record found." });
            return Ok(Issue);
        }

        [HttpGet("GetPreviousIssue/{id}")]
        public async Task<IActionResult> GetPreviousIssue(int id)
        {
            if (id == 0)
            {
                id = _service.GetMaxIssueId();
            }
            var Issue = await _service.GetPreviousIssue(id);
            if (Issue == null)
                return NotFound(new { Message = "No previous record found." });
            return Ok(Issue);
        }
    }
}
