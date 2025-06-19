using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Law.BL.Services.IServices;
using Law.CORE.Entities;
using Law.CORE.ViewModels;
using Law.DAL.Migrations;
using Law.DAL.Repository.IRepository;

namespace Law.BL.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReceiptService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Add(UpdateReceipt model)
        {
            Receipt receipt = new Receipt()
            {
                Code = model.Code,
                DateReceipt = model.DateReceipt,
                ClientId = model.ClientId,
                Amount = model.Amount,
                AmountName = model.AmountName,
                PaymentId = model.PaymentId,
                ChequeNumber = model.ChequeNumber,
                ChequeDate = model.ChequeDate,
                Purpose = model.Purpose,
                BankName = model.BankName,
                DesignType = model.DesignType,
                DesignNumber = model.DesignNumber,
                ProjectNumber = model.ProjectNumber,
                ProjectName = model.ProjectName,
            };
            if (await _unitOfWork.receipts.Add(receipt))
            {
                return "تم الحفظ بنجاح";
            }
            else
            {
                return "حدثت مشكلة اثناء الحفظ";
            }
        }

        public async Task<string> Delete(int id)
        {
            if (await _unitOfWork.receipts.Delete(id))
            {
                return "تم الحذف بنجاح";
            }
            else
            {
                return "حدثت مشكلة اثناء الحذف";
            }
        }

        public async Task<string> Edit(UpdateReceipt model)
        {
            Receipt receipt = new Receipt()
            {
                Id=model.Id,
                Code = model.Code,
                DateReceipt = model.DateReceipt,
                ClientId = model.ClientId,
                Amount = model.Amount,
                AmountName = model.AmountName,
                PaymentId = model.PaymentId,
                ChequeNumber = model.ChequeNumber,
                ChequeDate = model.ChequeDate,
                Purpose = model.Purpose,
                BankName = model.BankName,
                DesignType = model.DesignType,
                DesignNumber = model.DesignNumber,
                ProjectNumber = model.ProjectNumber,
                ProjectName = model.ProjectName,
            };
            if (await _unitOfWork.receipts.Update(receipt))
            {
                return "تم التعديل بنجاح";
            }
            else
            {
                return "حدثت مشكلة أثناء التعديل";
            }
        }

        public async Task<IEnumerable<Receipt>> GetAll()
        {
            return await _unitOfWork.receipts.GetAll();
        }

        public async Task<Receipt> GetbyId(int id)
        {
            return await _unitOfWork.receipts.GetById(id);
        }

        public async Task<ReceiptDto> GetMaxReceipt()
        {
            var receipt = await _unitOfWork.receipts.GetMax();
            if (receipt == null)
            {
                return new ReceiptDto();
            }
            ReceiptDto receiptDto=new ReceiptDto()
            {
                Id = receipt.Id,
                Code = receipt.Code,
                DateReceipt = receipt.DateReceipt,
                ClientId = receipt.ClientId,
                Amount = receipt.Amount,
                AmountName = receipt.AmountName,
                PaymentId = receipt.PaymentId,
                ChequeNumber = receipt.ChequeNumber,
                ChequeDate = receipt.ChequeDate,
                Purpose = receipt.Purpose,
                BankName = receipt.BankName,
                DesignType = receipt.DesignType,
                DesignNumber = receipt.DesignNumber,
                ProjectNumber = receipt.ProjectNumber,
                ProjectName = receipt.ProjectName

            };
            return receiptDto;
        }

        public int GetMaxReceiptId()
        {
            return _unitOfWork.receipts.GetMaxIdOfItem();
        }

        public async Task<ReceiptDto> GetMinReceipt()
        {
            var receipt = await _unitOfWork.receipts.GetMin();
            if (receipt == null)
            {
                return new ReceiptDto();
            }
            ReceiptDto receiptDto = new ReceiptDto()
            {
                Id = receipt.Id,
                Code = receipt.Code,
                DateReceipt = receipt.DateReceipt,
                ClientId = receipt.ClientId,
                Amount = receipt.Amount,
                AmountName = receipt.AmountName,
                PaymentId = receipt.PaymentId,
                ChequeNumber = receipt.ChequeNumber,
                ChequeDate = receipt.ChequeDate,
                Purpose = receipt.Purpose,
                BankName = receipt.BankName,
                DesignType = receipt.DesignType,
                DesignNumber = receipt.DesignNumber,
                ProjectNumber = receipt.ProjectNumber,
                ProjectName = receipt.ProjectName

            }; return receiptDto;
        }

        public async Task<int> GetNewCode()
        {
            return await _unitOfWork.receipts.GetNewCode();
        }

        public async Task<ReceiptDto> GetNextReceipt(int id)
        {
            var receipt = await _unitOfWork.receipts.GetNextOrPreviousItemByCode(id, "next");
            if (receipt == null)
            {
                return null;
            }
            ReceiptDto receiptDto = new ReceiptDto()
            {
                Id = receipt.Id,
                Code = receipt.Code,
                DateReceipt = receipt.DateReceipt,
                ClientId = receipt.ClientId,
                Amount = receipt.Amount,
                AmountName = receipt.AmountName,
                PaymentId = receipt.PaymentId,
                ChequeNumber = receipt.ChequeNumber,
                ChequeDate = receipt.ChequeDate,
                Purpose = receipt.Purpose,
                BankName = receipt.BankName,
                DesignType = receipt.DesignType,
                DesignNumber = receipt.DesignNumber,
                ProjectNumber = receipt.ProjectNumber,
                ProjectName = receipt.ProjectName

            }; return receiptDto;
        }
        public async Task<ReceiptDto> GetPreviousReceipt(int id)
        {
            var receipt = await _unitOfWork.receipts.GetNextOrPreviousItemByCode(id, "previous");
            if (receipt == null)
            {
                return null;
            }
            ReceiptDto receiptDto = new ReceiptDto()
            {
                Id = receipt.Id,
                Code = receipt.Code,
                DateReceipt = receipt.DateReceipt,
                ClientId = receipt.ClientId,
                Amount = receipt.Amount,
                AmountName = receipt.AmountName,
                PaymentId = receipt.PaymentId,
                ChequeNumber = receipt.ChequeNumber,
                ChequeDate = receipt.ChequeDate,
                Purpose = receipt.Purpose,
                BankName = receipt.BankName,
                DesignType = receipt.DesignType,
                DesignNumber = receipt.DesignNumber,
                ProjectNumber = receipt.ProjectNumber,
                ProjectName = receipt.ProjectName

            }; return receiptDto;
        }

        public async Task<PrintReceiptDto> GetReceiptForPrint(int id)
        {
            Receipt receiptDto =await _unitOfWork.receipts.GetById(id);
            if(receiptDto == null)
            {
                return new PrintReceiptDto();
            }
            if(receiptDto.ClientId != 0) 
            {
                string ClientName = "";
                var client =( await _unitOfWork.clients.GetById(receiptDto.ClientId));
                if(client == null)
                {
                    ClientName = "تم حذف العميل";
                }
                else
                {
                    ClientName = client.ClientName;
                }
                return MapToPrintReceiptDto(receiptDto, ClientName);
            }
            return new PrintReceiptDto();
        }

        private PrintReceiptDto MapToPrintReceiptDto(Receipt receipt, string clientName)
        {
            return new PrintReceiptDto
            {
                Id = receipt.Id,
                Code = receipt.Code,
                DateReceipt = receipt.DateReceipt,
                ClientName = clientName, // يجب تمريره من الخارج أو الحصول عليه باستخدام ClientId
                Amount = receipt.Amount,
                AmountName = receipt.AmountName,
                PaymentId=receipt.PaymentId,    
                PaymentName = receipt.PaymentId == 1 ? "شيك" : "نقدي",
                ChequeNumber = receipt.ChequeNumber,
                ChequeDate = receipt.ChequeDate,
                Purpose = receipt.Purpose,
                BankName = receipt.BankName,
                DesignType = receipt.DesignType == 1 ? "ابتدائي" : "نهائي",
                DesignNumber = receipt.DesignNumber,
                ProjectNumber = receipt.ProjectNumber,
                ProjectName = receipt.ProjectName
            };
        }

    }
}
