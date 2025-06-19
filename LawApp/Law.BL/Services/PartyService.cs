using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.BL.Services.IServices;
using Law.CORE.Entities;
using Law.CORE.ViewModels;
using Law.DAL.Repository.IRepository;

namespace Law.BL.Services
{
    public class PartyService : IPartyService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PartyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Add(UpdateParty model)
        {

            byte[] imageBytes;
            using (var ms = new MemoryStream())
            {
                await model.PartyImage.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }

            Party Party = new Party()
            {
                Code = model.Code,
                PartyName = model.PartyName,
                PartyImage = imageBytes,
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
            if (await _unitOfWork.parties.Add(Party))
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
            if (await _unitOfWork.parties.Delete(id))
            {
                return "تم الحذف بنجاح";
            }
            else
            {
                return "حدثت مشكلة اثناء الحذف";
            }
        }

        public async Task<string> Edit(UpdateParty model)
        {
            byte[] imageBytes;


            if (model.PartyImage != null && model.PartyImage.Length > 0)
            {
                // ✅ صورة جديدة تم رفعها
                using (var ms = new MemoryStream())
                {
                    await model.PartyImage.CopyToAsync(ms);
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

            Party Party = new Party()
            {
                Id = model.Id,
                Code = model.Code,
                PartyName = model.PartyName,
                PartyImage = imageBytes,
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

            if (await _unitOfWork.parties.Update(Party))
            {
                return "تم التعديل بنجاح";
            }
            else
            {
                return "حدثت مشكلة أثناء التعديل";
            }
        }

        public async Task<IEnumerable<Party>> GetAll()
        {
            return await _unitOfWork.parties.GetAll();
        }

        public async Task<Party> GetbyId(int id)
        {
            return await _unitOfWork.parties.GetById(id);
        }

        public async Task<PartyDto> GetMaxParty()
        {
            var Party = await _unitOfWork.parties.GetMax();
            if (Party == null)
                return null;

            return new PartyDto
            {
                Id = Party.Id,
                Code = Party.Code,
                PartyName = Party.PartyName,
                PartyImageBase64 = Party.PartyImage != null ? Convert.ToBase64String(Party.PartyImage) : null,
                WorkPhoneNumber = Party.WorkPhoneNumber,
                FaxNumber = Party.FaxNumber,
                MobileNumber = Party.MobileNumber,
                IssueDate = Party.IssueDate,
                Address = Party.Address,
                ResponsiblePerson = Party.ResponsiblePerson,
                NationalNumber = Party.NationalNumber,
                Source = Party.Source,
                DateNow = Party.DateNow,
                LastBalance = Party.LastBalance,
                Email = Party.Email
            };

        }


        public int GetMaxPartyId()
        {
            return _unitOfWork.parties.GetMaxIdOfItem();
        }

        public async Task<PartyDto?> GetMinParty()
        {
            var Party = await _unitOfWork.parties.GetMin();
            if (Party == null)
                return null;

            return new PartyDto
            {
                Id = Party.Id,
                Code = Party.Code,
                PartyName = Party.PartyName,
                PartyImageBase64 = Party.PartyImage != null ? Convert.ToBase64String(Party.PartyImage) : null,
                WorkPhoneNumber = Party.WorkPhoneNumber,
                FaxNumber = Party.FaxNumber,
                MobileNumber = Party.MobileNumber,
                IssueDate = Party.IssueDate,
                Address = Party.Address,
                ResponsiblePerson = Party.ResponsiblePerson,
                NationalNumber = Party.NationalNumber,
                Source = Party.Source,
                DateNow = Party.DateNow,
                LastBalance = Party.LastBalance,
                Email = Party.Email
            };
        }
        public async Task<int> GetNewCode()
        {
            return await _unitOfWork.parties.GetNewCode();
        }

        public async Task<PartyDto> GetNextParty(int id)
        {
            var Party = await _unitOfWork.parties.GetNextOrPreviousItemByCode(id, "next");
            if (Party == null)
                return null;

            return new PartyDto
            {
                Id = Party.Id,
                Code = Party.Code,
                PartyName = Party.PartyName,
                PartyImageBase64 = Party.PartyImage != null ? Convert.ToBase64String(Party.PartyImage) : null,

                WorkPhoneNumber = Party.WorkPhoneNumber,
                FaxNumber = Party.FaxNumber,
                MobileNumber = Party.MobileNumber,
                IssueDate = Party.IssueDate,
                Address = Party.Address,
                ResponsiblePerson = Party.ResponsiblePerson,
                NationalNumber = Party.NationalNumber,
                Source = Party.Source,
                DateNow = Party.DateNow,
                LastBalance = Party.LastBalance,
                Email = Party.Email
            };


        }

        public async Task<PartyDto> GetPreviousParty(int id)
        {
            var Party = await _unitOfWork.parties.GetNextOrPreviousItemByCode(id, "previous");
            if (Party == null)
                return null;

            return new PartyDto
            {
                Id = Party.Id,
                Code = Party.Code,
                PartyName = Party.PartyName,
                PartyImageBase64 = Party.PartyImage != null ? Convert.ToBase64String(Party.PartyImage) : null,
                WorkPhoneNumber = Party.WorkPhoneNumber,
                FaxNumber = Party.FaxNumber,
                MobileNumber = Party.MobileNumber,
                IssueDate = Party.IssueDate,
                Address = Party.Address,
                ResponsiblePerson = Party.ResponsiblePerson,
                NationalNumber = Party.NationalNumber,
                Source = Party.Source,
                DateNow = Party.DateNow,
                LastBalance = Party.LastBalance,
                Email = Party.Email
            };

        }
    }
}
