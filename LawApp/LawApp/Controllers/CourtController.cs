using Law.BL.Services.IServices;
using Law.CORE.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LawApp.Controllers
{
    public class CourtController : Controller
    {
        private readonly ICourtService _courtService;
        public CourtController(ICourtService courtService)
        {
            _courtService = courtService;   
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetNextCourtCode()
        {
            try
            {
                var nextCode = await _courtService.GetNewCode();
                return Json(new { nextCode });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Exception in GetNextCourtCode: " + ex.ToString());
                // Return full details for debugging (remove before production)
                return StatusCode(500, new { error = ex.Message, details = ex.ToString() });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddCourt([FromBody] CreateCourt model)
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
            var resultMessage = await _courtService.Add(model);
            // For this example, assume the service returns a success message string
            bool success = resultMessage.Contains("نجاح");

            return Ok(new { success, message = resultMessage });
        }
        [HttpPost]
        public async Task<IActionResult> EditCourt([FromBody] UpdateCourt model)
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

            var resultMessage = await _courtService.Edit(model);
            bool success = resultMessage.Contains("نجاح");

            return Ok(new { success, message = resultMessage });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCourt(int id)
        {
            try
            {
                string result = await _courtService.Delete(id);
                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("GetMinCourt")]
        public async Task<IActionResult> GetMinCourt()
        {
            var Court = await _courtService.GetMinCourt();
            if (Court == null)
                return NotFound(new { Message = "No records found." });
            return Ok(Court);
        }

        [HttpGet("GetMaxCourt")]
        public async Task<IActionResult> GetMaxCourt()
        {
            var Court = await _courtService.GetMaxCourt();
            if (Court == null)
                return NotFound(new { Message = "No records found." });
            return Ok(Court);
        }

        [HttpGet("GetNextCourt/{id}")]
        public async Task<IActionResult> GetNextCourt(int id)
        {
            if (id == 0)
            {
                id = _courtService.GetMaxCourtId();
            }
            var Court = await _courtService.GetNextCourt(id);
            if (Court == null)
                return NotFound(new { Message = "No next record found." });
            return Ok(Court);
        }

        [HttpGet("GetPreviousCourt/{id}")]
        public async Task<IActionResult> GetPreviousCourt(int id)
        {
            if (id == 0)
            {
                id = _courtService.GetMaxCourtId();
            }
            var Court = await _courtService.GetPreviousCourt(id);
            if (Court == null)
                return NotFound(new { Message = "No previous record found." });
            return Ok(Court);
        }
    }
}
