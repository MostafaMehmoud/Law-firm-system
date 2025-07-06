using Law.BL.Services.IServices;
using Law.CORE.ViewModels;
using LawApp.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace LawApp.Controllers
{
    public class PartyController : Controller
    {
        private readonly IPartyService _service;
        public PartyController(IPartyService service)
        {
            _service = service;
        }
        [Permission("CanAccessParty")]
        public IActionResult Index()
        {
            return View();
        }
        [ApiPermission("CanAccessParty")]
        public async Task<IActionResult> GetNextPartyCode()
        {
            try
            {
                var nextCode = await _service.GetNewCode();
                return Json(new { nextCode });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Exception in GetNextPartyCode: " + ex.ToString());
                // Return full details for debugging (remove before production)
                return StatusCode(500, new { error = ex.Message, details = ex.ToString() });
            }
        }
        [HttpPost]
        [ApiPermission("CanAccessParty")]
        public async Task<IActionResult> AddParty([FromForm] UpdateParty model)
        {
            ModelState.Remove("OldImageBase64");
            ModelState.Remove("Id");
            ModelState.Remove("DateNow");
            ModelState.Remove("LastBalance");
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
            var resultMessage = await _service.Add(model);
            // For this example, assume the service returns a success message string
            bool success = resultMessage.Contains("نجاح");

            return Ok(new { success, message = resultMessage });
        }
        [HttpPost]
        [ApiPermission("CanAccessParty")]
        public async Task<IActionResult> EditParty([FromForm] UpdateParty model)
        {
            ModelState.Remove("PartyImage");
            ModelState.Remove("OldImageBase64");
           
            ModelState.Remove("DateNow");
            ModelState.Remove("LastBalance");
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


            var resultMessage = await _service.Edit(model);
            bool success = resultMessage.Contains("نجاح");

            return Ok(new { success, message = resultMessage });
        }
        [HttpPost]
        [ApiPermission("CanAccessParty")]
        public async Task<IActionResult> DeleteParty(int id)
        {
            try
            {
                string result = await _service.Delete(id);
                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [ApiPermission("CanAccessParty")]
        [HttpGet("GetMinParty")]
        public async Task<IActionResult> GetMinParty()
        {
            var Party = await _service.GetMinParty();
            if (Party == null)
                return NotFound(new { Message = "No records found." });
            return Ok(Party);
        }

        [HttpGet("GetMaxParty")]
        [ApiPermission("CanAccessParty")]
        public async Task<IActionResult> GetMaxParty()
        {
            var Party = await _service.GetMaxParty();
            if (Party == null)
                return NotFound(new { Message = "No records found." });
            return Ok(Party);
        }

        [HttpGet("GetNextParty/{id}")]
        [ApiPermission("CanAccessParty")]
        public async Task<IActionResult> GetNextParty(int id)
        {
            if (id == 0)
            {
                id = _service.GetMaxPartyId();
            }
            var Party = await _service.GetNextParty(id);
            if (Party == null)
                return NotFound(new { Message = "No next record found." });
            return Ok(Party);
        }

        [HttpGet("GetPreviousParty/{id}")]
        [ApiPermission("CanAccessParty")]
        public async Task<IActionResult> GetPreviousParty(int id)
        {
            if (id == 0)
            {
                id = _service.GetMaxPartyId();
            }
            var Party = await _service.GetPreviousParty(id);
            if (Party == null)
                return NotFound(new { Message = "No previous record found." });
            return Ok(Party);
        }
    }
}
