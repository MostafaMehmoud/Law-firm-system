using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Law.CORE.ViewModels;

namespace Law.BL.Services.IServices
{
    public interface IReceiptService
    {
        public Task<string> Add(UpdateReceipt model);
        public Task<string> Edit(UpdateReceipt model);
        public Task<string> Delete(int id);
        public Task<Receipt> GetbyId(int id);
        public Task<IEnumerable<Receipt>> GetAll();
        public Task<int> GetNewCode();
        Task<ReceiptDto> GetNextReceipt(int id);
        Task<ReceiptDto> GetPreviousReceipt(int id);
        Task<ReceiptDto> GetMinReceipt();
        Task<ReceiptDto> GetMaxReceipt();
        int GetMaxReceiptId();
        public Task<PrintReceiptDto> GetReceiptForPrint(int id);
    }
}
