using Law.BL.Services;
using Law.BL.Services.IServices;
using Law.CORE.ViewModels;
using LawApp.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LawApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IPaymentService _paymentService;   
        public PaymentController(IClientService clientService, IPaymentService paymentService)
        {
            _clientService = clientService;
            _paymentService = paymentService;
        }
        [Permission("CanAccessPayments")]
        public async Task<IActionResult> Index()
        {
            ViewBag.listClients = new SelectList((await _clientService.GetAll()).ToList(), "Id", "ClientName");

            return View();
        }
        [ApiPermission("CanAccessPayments")]
        public async Task<IActionResult> GetNextPaymentCode()
        {
            try
            {
                var nextCode = await _paymentService.GetNewCode();
                return Json(new { nextCode });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Exception in GetNextPaymentCode: " + ex.ToString());
                // Return full details for debugging (remove before production)
                return StatusCode(500, new { error = ex.Message, details = ex.ToString() });
            }
        }
        [HttpPost]
        [ApiPermission("CanAccessPayments")]
        public async Task<IActionResult> AddPayment([FromForm] UpdatePayment model)
        {

            ModelState.Remove("Id");
            ModelState.Remove("DatePayment");
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
            var resultMessage = await _paymentService.Add(model);
            // For this example, assume the service returns a success message string
            bool success = resultMessage.Contains("نجاح");

            return Ok(new { success, message = resultMessage });
        }
        [HttpPost]
        [ApiPermission("CanAccessPayments")]
        public async Task<IActionResult> EditPayment([FromForm] UpdatePayment model)
        {
            ModelState.Remove("PaymentImage");
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


            var resultMessage = await _paymentService.Edit(model);
            bool success = resultMessage.Contains("نجاح");

            return Ok(new { success, message = resultMessage });
        }
        [HttpPost]
        [ApiPermission("CanAccessPayments")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                string result = await _paymentService.Delete(id);
                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("GetMinPayment")]
        [ApiPermission("CanAccessPayments")]
        public async Task<IActionResult> GetMinPayment()
        {
            var Payment = await _paymentService.GetMinPayment();
            if (Payment == null)
                return NotFound(new { Message = "No records found." });
            return Ok(Payment);
        }

        [HttpGet("GetMaxPayment")]
        [ApiPermission("CanAccessPayments")]
        public async Task<IActionResult> GetMaxPayment()
        {
            var Payment = await _paymentService.GetMaxPayment();
            if (Payment == null)
                return NotFound(new { Message = "No records found." });
            return Ok(Payment);
        }

        [HttpGet("GetNextPayment/{id}")]
        [ApiPermission("CanAccessPayments")]
        public async Task<IActionResult> GetNextPayment(int id)
        {
            if (id == 0)
            {
                id = _paymentService.GetMaxPaymentId();
            }
            var Payment = await _paymentService.GetNextPayment(id);
            if (Payment == null)
                return NotFound(new { Message = "No next record found." });
            return Ok(Payment);
        }

        [HttpGet("GetPreviousPayment/{id}")]
        [ApiPermission("CanAccessPayments")]
        public async Task<IActionResult> GetPreviousPayment(int id)
        {
            if (id == 0)
            {
                id = _paymentService.GetMaxPaymentId();
            }
            var Payment = await _paymentService.GetPreviousPayment(id);
            if (Payment == null)
                return NotFound(new { Message = "No previous record found." });
            return Ok(Payment);
        }
        [Permission("CanAccessPayments")]
        public async Task<IActionResult> PrintPayment(int id)
        {
            var model = await _paymentService.GetpaymentForPrint(id); // اجلب البيانات
            return View(model); // صفحة PrintPayment.cshtml
        }
    }
}
