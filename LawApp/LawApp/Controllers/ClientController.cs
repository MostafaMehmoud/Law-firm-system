using System;
using Law.BL.Services.IServices;
using Law.CORE.ViewModels;
using LawApp.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace LawApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _service;
        public ClientController(IClientService service)
        {
            _service = service;
        }
        [Permission("CanAccessClient")]
        public IActionResult Index()
        {
            return View();
        }
        [ApiPermission("CanAccessClient")]
        public async Task<IActionResult> GetNextClientCode()
        {
            try
            {
                var nextCode = await _service.GetNewCode();
                return Json(new { nextCode });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Exception in GetNextClientCode: " + ex.ToString());
                // Return full details for debugging (remove before production)
                return StatusCode(500, new { error = ex.Message, details = ex.ToString() });
            }
        }
        [HttpPost]
        [ApiPermission("CanAccessClient")]
        public async Task<IActionResult> AddClient([FromForm] UpdateClient model)
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
        [ApiPermission("CanAccessClient")]
        public async Task<IActionResult> EditClient([FromForm] UpdateClient model)
        {
            ModelState.Remove("ClientImage");
            ModelState.Remove("OldImageBase64");
            ModelState.Remove("Id");
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
        [ApiPermission("CanAccessClient")]
        public async Task<IActionResult> DeleteClient(int id)
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
        [HttpGet("GetMinClient")]
        [ApiPermission("CanAccessClient")]
        public async Task<IActionResult> GetMinClient()
        {
            var client = await _service.GetMinClient();
            if (client == null)
                return NotFound(new { Message = "No records found." });
            return Ok(client);
        }

        [HttpGet("GetMaxClient")]
        [ApiPermission("CanAccessClient")]
        public async Task<IActionResult> GetMaxClient()
        {
            var Client = await _service.GetMaxClient();
            if (Client == null)
                return NotFound(new { Message = "No records found." });
            return Ok(Client);
        }

        [HttpGet("GetNextClient/{id}")]
        [ApiPermission("CanAccessClient")]
        public async Task<IActionResult> GetNextClient(int id)
        {
            if (id == 0)
            {
                id = _service.GetMaxClientId();
            }
            var Client = await _service.GetNextClient(id);
            if (Client == null)
                return NotFound(new { Message = "No next record found." });
            return Ok(Client);
        }

        [HttpGet("GetPreviousClient/{id}")]
        [ApiPermission("CanAccessClient")]
        public async Task<IActionResult> GetPreviousClient(int id)
        {
            if (id == 0)
            {
                id = _service.GetMaxClientId();
            }
            var Client = await _service.GetPreviousClient(id);
            if (Client == null)
                return NotFound(new { Message = "No previous record found." });
            return Ok(Client);
        }
    }
}
