using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Law.BL.Services.IServices;
using Law.CORE.Entities;
using Law.CORE.ViewModels;
using Law.DAL.Migrations;
using Law.DAL.Repository.IRepository;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;

namespace Law.BL.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }
        public  async Task<string> Add(UpdateClient model)
        {

            byte[] imageBytes;
            using (var ms = new MemoryStream())
            {
               await model.ClientImage.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }

            Client client = new Client()
            {
                Code = model.Code,
                ClientName = model.ClientName,
                ClientImage = imageBytes,
                WorkPhoneNumber = model.WorkPhoneNumber,
                FaxNumber = model.FaxNumber,
                MobileNumber = model.MobileNumber,
                IssueDate = (DateOnly)model.IssueDate,
                Address = model.Address,
                ResponsiblePerson = model.ResponsiblePerson,
                NationalNumber = model.NationalNumber,
                DateNow = model.DateNow,
                LastBalance = model.LastBalance,
                Email = model.Email,
                Source=model.Source,
            };
            if (await _unitOfWork.clients.Add(client))
            {
                return "تم الحفظ بنجاح";
            }
            else
            {
                return "حدثت مشكلة اثناء الحفظ";
            }

        }

        public  async Task<string> Delete(int id)
        {
            if (await _unitOfWork.clients.Delete(id))
            {
                return "تم الحذف بنجاح";
            }
            else
            {
                return "حدثت مشكلة اثناء الحذف";
            }
        }

        public async Task<string> Edit(UpdateClient model)
        {
            byte[] imageBytes;


            if (model.ClientImage != null && model.ClientImage.Length > 0)
            {
                // ✅ صورة جديدة تم رفعها
                using (var ms = new MemoryStream())
                {
                    await model.ClientImage.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }
            }
            else if (!string.IsNullOrEmpty(model.OldImageBase64))
            {
                // ✅ لم يتم رفع صورة جديدة ⇒ استخدم القديمة (Base64)
                try
                {
                    imageBytes = Convert.FromBase64String(model.OldImageBase64);
                }
                catch
                {
                    return "حدثت مشكلة أثناء تحويل الصورة القديمة.";
                }
            }
            else
            {
                // ❌ لا صورة جديدة ولا قديمة
                return "الرجاء تحديد صورة.";
            }

            Client client = new Client()
            {
                Id=model.Id,
                Code = model.Code,
                ClientName = model.ClientName,
                ClientImage = imageBytes,
                WorkPhoneNumber = model.WorkPhoneNumber,
                FaxNumber = model.FaxNumber,
                MobileNumber = model.MobileNumber,
                IssueDate = (DateOnly)model.IssueDate,
                Address = model.Address,
                ResponsiblePerson = model.ResponsiblePerson,
                NationalNumber = model.NationalNumber,
                DateNow = model.DateNow,
                LastBalance = model.LastBalance,
                Email = model.Email,
                Source = model.Source,
            };

            if (await _unitOfWork.clients.Update(client))
            {
                return "تم التعديل بنجاح";
            }
            else
            {
                return "حدثت مشكلة أثناء التعديل";
            }
        }

        public  async Task<IEnumerable<Client>> GetAll()
        {
            return await _unitOfWork.clients.GetAll();
        }

        public  async Task<Client> GetbyId(int id)
        {
            return await _unitOfWork.clients.GetById(id);
        }

        public  async Task<ClientDto> GetMaxClient()
        {
            var client = await _unitOfWork.clients.GetMax();
            if (client == null)
                return null;

            return new ClientDto
            {
                Id = client.Id,
                Code = client.Code,
                ClientName = client.ClientName,
                ClientImageBase64 = client.ClientImage != null ? Convert.ToBase64String(client.ClientImage) : null,
                WorkPhoneNumber = client.WorkPhoneNumber,
                FaxNumber = client.FaxNumber,
                MobileNumber = client.MobileNumber,
                IssueDate = client.IssueDate,
                Address = client.Address,
                ResponsiblePerson = client.ResponsiblePerson,
                NationalNumber = client.NationalNumber,
                Source = client.Source,
                DateNow = client.DateNow,
                LastBalance = client.LastBalance,
                Email = client.Email
            };
           
        }
        

        public int GetMaxClientId()
        {
            return  _unitOfWork.clients.GetMaxIdOfItem();
        }

        public async Task<ClientDto?> GetMinClient()
        {
            var client = await _unitOfWork.clients.GetMin();
            if (client == null)
                return null;

            return new ClientDto
            {
                Id = client.Id,
                Code = client.Code,
                ClientName = client.ClientName,
                ClientImageBase64 = client.ClientImage != null ? Convert.ToBase64String(client.ClientImage) : null,
                WorkPhoneNumber = client.WorkPhoneNumber,
                FaxNumber = client.FaxNumber,
                MobileNumber = client.MobileNumber,
                IssueDate = client.IssueDate,
                Address = client.Address,
                ResponsiblePerson = client.ResponsiblePerson,
                NationalNumber = client.NationalNumber,
                Source = client.Source,
                DateNow = client.DateNow,
                LastBalance = client.LastBalance,
                Email = client.Email
            };
        }
        public  async Task<int> GetNewCode()
        {
            return await _unitOfWork.clients.GetNewCode();
        }

        public  async Task<ClientDto> GetNextClient(int id)
        {
            var client = await _unitOfWork.clients.GetNextOrPreviousItemByCode(id, "next");
            if (client == null)
                return null;

            return new ClientDto
            {
                Id = client.Id,
                Code = client.Code,
                ClientName = client.ClientName,
                ClientImageBase64 = client.ClientImage != null ? Convert.ToBase64String(client.ClientImage) : null,
                
                WorkPhoneNumber = client.WorkPhoneNumber,
                FaxNumber = client.FaxNumber,
                MobileNumber = client.MobileNumber,
                IssueDate = client.IssueDate,
                Address = client.Address,
                ResponsiblePerson = client.ResponsiblePerson,
                NationalNumber = client.NationalNumber,
                Source = client.Source,
                DateNow = client.DateNow,
                LastBalance = client.LastBalance,
                Email = client.Email
            };

           
        }

        public  async Task<ClientDto> GetPreviousClient(int id)
        {
            var client = await _unitOfWork.clients.GetNextOrPreviousItemByCode(id, "previous");
            if (client == null)
                return null;

            return new ClientDto
            {
                Id = client.Id,
                Code = client.Code,
                ClientName = client.ClientName,
                ClientImageBase64 = client.ClientImage != null ? Convert.ToBase64String(client.ClientImage) : null,
                WorkPhoneNumber = client.WorkPhoneNumber,
                FaxNumber = client.FaxNumber,
                MobileNumber = client.MobileNumber,
                IssueDate = client.IssueDate,
                Address = client.Address,
                ResponsiblePerson = client.ResponsiblePerson,
                NationalNumber = client.NationalNumber,
                Source = client.Source,
                DateNow = client.DateNow,
                LastBalance = client.LastBalance,
                Email = client.Email
            };
           
        }
    }
}
