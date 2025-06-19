using Law.BL.Services.IServices;
using Law.CORE.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LawApp.Controllers
{
    public class ReceiptController : Controller
    {
        private readonly IReceiptService _receiptService;
        private readonly IClientService _clientService;
        public ReceiptController(IReceiptService receiptService, IClientService clientService)
        {
            _receiptService = receiptService;
            _clientService = clientService;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.listClients=new SelectList((await _clientService.GetAll()).ToList(), "Id", "ClientName");
            return View();
        }
        public async Task<IActionResult> GetNextReceiptCode()
        {
            try
            {
                var nextCode = await _receiptService.GetNewCode();
                return Json(new { nextCode });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Exception in GetNextReceiptCode: " + ex.ToString());
                // Return full details for debugging (remove before production)
                return StatusCode(500, new { error = ex.Message, details = ex.ToString() });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddReceipt([FromForm] UpdateReceipt model)
        {
           
            ModelState.Remove("Id");
            ModelState.Remove("DateReceipt");
            ModelState.Remove("ProjectNumber");
            ModelState.Remove("DesignNumber");
            
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
            var resultMessage = await _receiptService.Add(model);
            // For this example, assume the service returns a success message string
            bool success = resultMessage.Contains("نجاح");

            return Ok(new { success, message = resultMessage });
        }
        [HttpPost]
        public async Task<IActionResult> EditReceipt([FromForm] UpdateReceipt model)
        {
            ModelState.Remove("ReceiptImage");
            ModelState.Remove("OldImageBase64");
            ModelState.Remove("ProjectNumber");
            ModelState.Remove("DesignNumber");
          
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


            var resultMessage = await _receiptService.Edit(model);
            bool success = resultMessage.Contains("نجاح");

            return Ok(new { success, message = resultMessage });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteReceipt(int id)
        {
            try
            {
                string result = await _receiptService.Delete(id);
                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("GetMinReceipt")]
        public async Task<IActionResult> GetMinReceipt()
        {
            var Receipt = await _receiptService.GetMinReceipt();
            if (Receipt == null)
                return NotFound(new { Message = "No records found." });
            return Ok(Receipt);
        }

        [HttpGet("GetMaxReceipt")]
        public async Task<IActionResult> GetMaxReceipt()
        {
            var Receipt = await _receiptService.GetMaxReceipt();
            if (Receipt == null)
                return NotFound(new { Message = "No records found." });
            return Ok(Receipt);
        }

        [HttpGet("GetNextReceipt/{id}")]
        public async Task<IActionResult> GetNextReceipt(int id)
        {
            if (id == 0)
            {
                id = _receiptService.GetMaxReceiptId();
            }
            var Receipt = await _receiptService.GetNextReceipt(id);
            if (Receipt == null)
                return NotFound(new { Message = "No next record found." });
            return Ok(Receipt);
        }

        [HttpGet("GetPreviousReceipt/{id}")]
        public async Task<IActionResult> GetPreviousReceipt(int id)
        {
            if (id == 0)
            {
                id = _receiptService.GetMaxReceiptId();
            }
            var Receipt = await _receiptService.GetPreviousReceipt(id);
            if (Receipt == null)
                return NotFound(new { Message = "No previous record found." });
            return Ok(Receipt);
        }
        public async Task<IActionResult> PrintReceipt(int id)
        {
            var model =await _receiptService.GetReceiptForPrint(id); // اجلب البيانات
            return View(model); // صفحة PrintReceipt.cshtml
        }

    }
}
