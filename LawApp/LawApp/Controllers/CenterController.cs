using Law.BL.Services.IServices;
using Law.CORE.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LawApp.Controllers
{
    public class CenterController : Controller
    {
        private readonly ICourtService _courtService;
        private readonly ICenterService _centerService;
        public CenterController(ICourtService courtService, ICenterService centerService)
        {
            _centerService = centerService;
            _courtService = courtService;
        }
        public async Task<IActionResult> Index()
        {
            var listcourt = await _courtService.GetAll();
            ViewBag.listCourts = new SelectList(listcourt.ToList(), "Id", "CourtName");
            return View();
        }
        public async Task<IActionResult> GetNextCenterCode()
        {
            try
            {
                var nextCode = await _centerService.GetNewCode();
                return Json(new { nextCode });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Exception in GetNextCenterCode: " + ex.ToString());
                // Return full details for debugging (remove before production)
                return StatusCode(500, new { error = ex.Message, details = ex.ToString() });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddCenter([FromBody] CreateCenter model)
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
            var resultMessage = await _centerService.Add(model);
            // For this example, assume the service returns a success message string
            bool success = resultMessage.Contains("نجاح");

            return Ok(new { success, message = resultMessage });
        }
        [HttpPost]
        public async Task<IActionResult> EditCenter([FromBody] UpdateCenter model)
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

            var resultMessage = await _centerService.Edit(model);
            bool success = resultMessage.Contains("نجاح");

            return Ok(new { success, message = resultMessage });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCenter(int id)
        {
            try
            {
                string result = await _centerService.Delete(id);
                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("GetMinCenter")]
        public async Task<IActionResult> GetMinCenter()
        {
            var Center = await _centerService.GetMinCenter();
            if (Center == null)
                return NotFound(new { Message = "No records found." });
            return Ok(Center);
        }

        [HttpGet("GetMaxCenter")]
        public async Task<IActionResult> GetMaxCenter()
        {
            var Center = await _centerService.GetMaxCenter();
            if (Center == null)
                return NotFound(new { Message = "No records found." });
            return Ok(Center);
        }

        [HttpGet("GetNextCenter/{id}")]
        public async Task<IActionResult> GetNextCenter(int id)
        {
            if (id == 0)
            {
                id = _centerService.GetMaxCenterId();
            }
            var Center = await _centerService.GetNextCenter(id);
            if (Center == null)
                return NotFound(new { Message = "No next record found." });
            return Ok(Center);
        }

        [HttpGet("GetPreviousCenter/{id}")]
        public async Task<IActionResult> GetPreviousCenter(int id)
        {
            if (id == 0)
            {
                id = _centerService.GetMaxCenterId();
            }
            var Center = await _centerService.GetPreviousCenter(id);
            if (Center == null)
                return NotFound(new { Message = "No previous record found." });
            return Ok(Center);
        }
    }
}
