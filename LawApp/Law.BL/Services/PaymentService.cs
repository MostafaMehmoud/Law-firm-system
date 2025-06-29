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
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        public async Task<string> Add(UpdatePayment model)
        {
            Payment payment = new Payment()
            {
                Code = model.Code,
                DatePayment = model.DatePayment,
                ClientId = model.ClientId,
                Amount = model.Amount,
                AmountName = model.AmountName,
                PaymentId = model.PaymentId,
                ChequeNumber = model.ChequeNumber,
                ChequeDate = model.ChequeDate,
                Purpose = model.Purpose,
                BankName = model.BankName,
                ProjectNumber = model.ProjectNumber,
                ProjectName = model.ProjectName,
                MoneyTypeName=model.MoneyTypeName
            };
            if (await _unitOfWork.payments.Add(payment))
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
            if (await _unitOfWork.payments.Delete(id))
            {
                return "تم الحذف بنجاح";
            }
            else
            {
                return "حدثت مشكلة اثناء الحذف";
            }
        }

        public async Task<string> Edit(UpdatePayment model)
        {
            Payment payment = new Payment()
            {
                Id = model.Id,
                Code = model.Code,
                DatePayment = model.DatePayment,
                ClientId = model.ClientId,
                Amount = model.Amount,
                AmountName = model.AmountName,
                PaymentId = model.PaymentId,
                ChequeNumber = model.ChequeNumber,
                ChequeDate = model.ChequeDate,
                Purpose = model.Purpose,
                BankName = model.BankName,
                ProjectNumber = model.ProjectNumber,
                ProjectName = model.ProjectName,
                MoneyTypeName = model.MoneyTypeName
            };
            if (await _unitOfWork.payments.Update(payment))
            {
                return "تم التعديل بنجاح";
            }
            else
            {
                return "حدثت مشكلة أثناء التعديل";
            }
        }

        public async Task<IEnumerable<Payment>> GetAll()
        {
            return await _unitOfWork.payments.GetAll();
        }

        public async Task<Payment> GetbyId(int id)
        {
            return await _unitOfWork.payments.GetById(id);
        }

        public async Task<PaymentDto> GetMaxPayment()
        {
            var Payment = await _unitOfWork.payments.GetMax();
            if (Payment == null)
            {
                return new PaymentDto();
            }
            PaymentDto PaymentDto = new PaymentDto()
            {
                Id = Payment.Id,
                Code = Payment.Code,
                DatePayment = Payment.DatePayment,
                ClientId = Payment.ClientId,
                Amount = Payment.Amount,
                AmountName = Payment.AmountName,
                PaymentId = Payment.PaymentId,
                ChequeNumber = Payment.ChequeNumber,
                ChequeDate = Payment.ChequeDate,
                Purpose = Payment.Purpose,
                BankName = Payment.BankName,
                ProjectNumber = Payment.ProjectNumber,
                ProjectName = Payment.ProjectName,
                MoneyTypeName= Payment.MoneyTypeName,

            };
            return PaymentDto;
        }

        public int GetMaxPaymentId()
        {
            return _unitOfWork.payments.GetMaxIdOfItem();
        }

        public async Task<PaymentDto> GetMinPayment()
        {
            var Payment = await _unitOfWork.payments.GetMin();
            if (Payment == null)
            {
                return new PaymentDto();
            }
            PaymentDto PaymentDto = new PaymentDto()
            {
                Id = Payment.Id,
                Code = Payment.Code,
                DatePayment = Payment.DatePayment,
                ClientId = Payment.ClientId,
                Amount = Payment.Amount,
                AmountName = Payment.AmountName,
                PaymentId = Payment.PaymentId,
                ChequeNumber = Payment.ChequeNumber,
                ChequeDate = Payment.ChequeDate,
                Purpose = Payment.Purpose,
                BankName = Payment.BankName,
                ProjectNumber = Payment.ProjectNumber,
                ProjectName = Payment.ProjectName,
                MoneyTypeName = Payment.MoneyTypeName,

            };
            return PaymentDto;
        }

        public async Task<int> GetNewCode()
        {
            return await _unitOfWork.payments.GetNewCode();
        }

        public async Task<PaymentDto> GetNextPayment(int id)
        {
            var Payment = await _unitOfWork.payments.GetNextOrPreviousItemByCode(id, "next");
            if (Payment == null)
            {
                return new PaymentDto();
            }
            PaymentDto PaymentDto = new PaymentDto()
            {
                Id = Payment.Id,
                Code = Payment.Code,
                DatePayment = Payment.DatePayment,
                ClientId = Payment.ClientId,
                Amount = Payment.Amount,
                AmountName = Payment.AmountName,
                PaymentId = Payment.PaymentId,
                ChequeNumber = Payment.ChequeNumber,
                ChequeDate = Payment.ChequeDate,
                Purpose = Payment.Purpose,
                BankName = Payment.BankName,
                ProjectNumber = Payment.ProjectNumber,
                ProjectName = Payment.ProjectName,
                MoneyTypeName = Payment.MoneyTypeName,

            };
            return PaymentDto;
        }

        public async Task<PaymentDto> GetPreviousPayment(int id)
        {
            var Payment = await _unitOfWork.payments.GetNextOrPreviousItemByCode(id, "previous"); 
            if (Payment == null)
            {
                return new PaymentDto();
            }
            PaymentDto PaymentDto = new PaymentDto()
            {
                Id = Payment.Id,
                Code = Payment.Code,
                DatePayment = Payment.DatePayment,
                ClientId = Payment.ClientId,
                Amount = Payment.Amount,
                AmountName = Payment.AmountName,
                PaymentId = Payment.PaymentId,
                ChequeNumber = Payment.ChequeNumber,
                ChequeDate = Payment.ChequeDate,
                Purpose = Payment.Purpose,
                BankName = Payment.BankName,
                ProjectNumber = Payment.ProjectNumber,
                ProjectName = Payment.ProjectName,
                MoneyTypeName = Payment.MoneyTypeName,

            };
            return PaymentDto;
        }
        public async Task<PrintPaymentDto> GetpaymentForPrint(int id)
        {
            Payment PaymentDto = await _unitOfWork.payments.GetById(id);
            if (PaymentDto == null)
            {
                return new PrintPaymentDto();
            }
            if (PaymentDto.ClientId != 0)
            {
                string ClientName = "";
                var client = (await _unitOfWork.clients.GetById(PaymentDto.ClientId));
                if (client == null)
                {
                    ClientName = "تم حذف العميل";
                }
                else
                {
                    ClientName = client.ClientName;
                }
                return MapToPrintPaymentDto(PaymentDto, ClientName);
            }
            return new PrintPaymentDto();
        }

        private PrintPaymentDto MapToPrintPaymentDto(Payment Payment, string clientName)
        {
            return new PrintPaymentDto
            {
                Id = Payment.Id,
                Code = Payment.Code,
                DatePayment = Payment.DatePayment,
                ClientName = clientName, // يجب تمريره من الخارج أو الحصول عليه باستخدام ClientId
                Amount = Payment.Amount,
                AmountName = Payment.AmountName,
                PaymentId = Payment.PaymentId,
                PaymentName = Payment.PaymentId == 1 ? "شيك" : "نقدي",
                ChequeNumber = Payment.ChequeNumber,
                ChequeDate = Payment.ChequeDate,
                Purpose = Payment.Purpose,
                BankName = Payment.BankName,
               MoneyTypeName=Payment.MoneyTypeName,
                ProjectNumber = Payment.ProjectNumber,
                ProjectName = Payment.ProjectName
            };
        }
    }
}
