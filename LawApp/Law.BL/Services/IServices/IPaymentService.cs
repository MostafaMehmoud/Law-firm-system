using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Law.CORE.ViewModels;

namespace Law.BL.Services.IServices
{
    public interface IPaymentService
    {
        public Task<string> Add(UpdatePayment model);
        public Task<string> Edit(UpdatePayment model);
        public Task<string> Delete(int id);
        public Task<Payment> GetbyId(int id);
        public Task<IEnumerable<Payment>> GetAll();
        public Task<int> GetNewCode();
        Task<PaymentDto> GetNextPayment(int id);
        Task<PaymentDto> GetPreviousPayment(int id);
        Task<PaymentDto> GetMinPayment();
        Task<PaymentDto> GetMaxPayment();
        int GetMaxPaymentId();
        public Task<PrintPaymentDto> GetpaymentForPrint(int id);
    }
}
