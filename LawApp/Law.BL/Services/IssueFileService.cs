using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Law.BL.Services.IServices;
using Law.CORE.Entities;
using Law.CORE.ViewModels;
using Law.DAL.Migrations;
using Law.DAL.Repository.IRepository;

namespace Law.BL.Services
{
    public class IssueFileService : IIssueFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        public IssueFileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        public async Task<string> Add(UpdateIssueFile model)
        {
            byte[] imageBytes;
            using (var ms = new MemoryStream())
            {
                await model.IssueImage.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }
            IssueFile issueFile = new IssueFile()
            {
                Code= model.Code,
                IssueNumber=model.IssueNumber,
                IssueName=model.IssueName,
                DateNow=model.DateNow,
                ClientId=model.ClientId,
                ClientProperty=model.ClientProperty,
                ClientPhoneNumber=model.ClientPhoneNumber,
                ClientAddress=model.ClientAddress,
                PartyId=model.PartyId,
                PartyPhoneNumber=model.PartyPhoneNumber,
                PartyAddress=model.PartyAddress,
                PartyWorkPlace=model.PartyWorkPlace,
                IssueTypeId=model.IssueTypeId,
                IssueValueFees=model.IssueValueFees,
                IssueDescription=model.IssueDescription,
                IssueDegreeNegotiation=model.IssueDegreeNegotiation,
                CourtId=model.CourtId,
                ClaimNumber=model.ClaimNumber,
                YearOfIssue=model.YearOfIssue,
                CenterId=model.CenterId,
                RuleOfIssue=model.RuleOfIssue,
                DateNextSession=model.DateNextSession,
                WhatHappenedInTheCourtSession=model.WhatHappenedInTheCourtSession,
                IssueImage= imageBytes
            };
            if (await _unitOfWork.issuesFile.Add(issueFile))
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
            if (await _unitOfWork.issuesFile.Delete(id))
            {
                return "تم الحذف بنجاح";
            }
            else
            {
                return "حدثت مشكلة اثناء الحذف";
            }
        }

        public async Task<string> Edit(UpdateIssueFile model)
        {
            byte[] imageBytes;


            if (model.IssueImage != null && model.IssueImage.Length > 0)
            {
                // ✅ صورة جديدة تم رفعها
                using (var ms = new MemoryStream())
                {
                    await model.IssueImage.CopyToAsync(ms);
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

            IssueFile issueFile = new IssueFile()
            {
                Id=model.Id,
                Code = model.Code,
                IssueNumber = model.IssueNumber,
                IssueName = model.IssueName,
                DateNow = model.DateNow,
                ClientId = model.ClientId,
                ClientProperty = model.ClientProperty,
                ClientPhoneNumber = model.ClientPhoneNumber,
                ClientAddress = model.ClientAddress,
                PartyId = model.PartyId,
                PartyPhoneNumber = model.PartyPhoneNumber,
                PartyAddress = model.PartyAddress,
                PartyWorkPlace = model.PartyWorkPlace,
                IssueTypeId = model.IssueTypeId,
                IssueValueFees = model.IssueValueFees,
                IssueDescription = model.IssueDescription,
                IssueDegreeNegotiation = model.IssueDegreeNegotiation,
                CourtId = model.CourtId,
                ClaimNumber = model.ClaimNumber,
                YearOfIssue = model.YearOfIssue,
                CenterId = model.CenterId,
                RuleOfIssue = model.RuleOfIssue,
                DateNextSession = model.DateNextSession,
                WhatHappenedInTheCourtSession = model.WhatHappenedInTheCourtSession,
                IssueImage = imageBytes
            };

            if (await _unitOfWork.issuesFile.Update(issueFile))
            {
                return "تم التعديل بنجاح";
            }
            else
            {
                return "حدثت مشكلة أثناء التعديل";
            }
        }

        public async Task<IEnumerable<IssueFile>> GetAll()
        {
            return await _unitOfWork.issuesFile.GetAll();   
        }

        public async Task<IssueFile> GetbyId(int id)
        {
            return await _unitOfWork.issuesFile.GetById(id);
        }

        public async Task<IssueFileDto> GetMaxIssueFile()
        {
            var issueFile = await _unitOfWork.issuesFile.GetMax();
            if (issueFile == null)
                return null;
            return new IssueFileDto
            {
                Id = issueFile.Id,
                Code = issueFile.Code,
                IssueNumber = issueFile.IssueNumber,
                IssueName = issueFile.IssueName,
                DateNow = issueFile.DateNow,
                ClientId = issueFile.ClientId,
                ClientProperty = issueFile.ClientProperty,
                ClientPhoneNumber = issueFile.ClientPhoneNumber,
                ClientAddress = issueFile.ClientAddress,
                PartyId = issueFile.PartyId,
                PartyPhoneNumber = issueFile.PartyPhoneNumber,
                PartyAddress = issueFile.PartyAddress,
                PartyWorkPlace = issueFile.PartyWorkPlace,
                IssueTypeId = issueFile.IssueTypeId,
                IssueValueFees = issueFile.IssueValueFees,
                IssueDescription = issueFile.IssueDescription,
                IssueDegreeNegotiation = issueFile.IssueDegreeNegotiation,
                CourtId = issueFile.CourtId,
                ClaimNumber = issueFile.ClaimNumber,
                YearOfIssue = issueFile.YearOfIssue,
                CenterId = issueFile.CenterId,
                RuleOfIssue = issueFile.RuleOfIssue,
                DateNextSession = issueFile.DateNextSession,
                WhatHappenedInTheCourtSession = issueFile.WhatHappenedInTheCourtSession,
            ClientImageBase64 = issueFile.IssueImage != null ? Convert.ToBase64String(issueFile.IssueImage) : null,

            };
        }

        public int GetMaxIssueFileId()
        {
            return _unitOfWork.issuesFile.GetMaxIdOfItem();
        }

        public async Task<IssueFileDto> GetMinIssueFile()
        {
            var issueFile = await _unitOfWork.issuesFile.GetMin();
            if (issueFile == null)
                return null;
            return new IssueFileDto
            {
                Id = issueFile.Id,
                Code = issueFile.Code,
                IssueNumber = issueFile.IssueNumber,
                IssueName = issueFile.IssueName,
                DateNow = issueFile.DateNow,
                ClientId = issueFile.ClientId,
                ClientProperty = issueFile.ClientProperty,
                ClientPhoneNumber = issueFile.ClientPhoneNumber,
                ClientAddress = issueFile.ClientAddress,
                PartyId = issueFile.PartyId,
                PartyPhoneNumber = issueFile.PartyPhoneNumber,
                PartyAddress = issueFile.PartyAddress,
                PartyWorkPlace = issueFile.PartyWorkPlace,
                IssueTypeId = issueFile.IssueTypeId,
                IssueValueFees = issueFile.IssueValueFees,
                IssueDescription = issueFile.IssueDescription,
                IssueDegreeNegotiation = issueFile.IssueDegreeNegotiation,
                CourtId = issueFile.CourtId,
                ClaimNumber = issueFile.ClaimNumber,
                YearOfIssue = issueFile.YearOfIssue,
                CenterId = issueFile.CenterId,
                RuleOfIssue = issueFile.RuleOfIssue,
                DateNextSession = issueFile.DateNextSession,
                WhatHappenedInTheCourtSession = issueFile.WhatHappenedInTheCourtSession,
                ClientImageBase64 = issueFile.IssueImage != null ? Convert.ToBase64String(issueFile.IssueImage) : null,

            };
        }

        public async Task<int> GetNewCode()
        {
            return await _unitOfWork.issuesFile.GetNewCode();
        }

        public async Task<IssueFileDto> GetNextIssueFile(int id)
        {
            var issueFile = await _unitOfWork.issuesFile.GetNextOrPreviousItemByCode(id, "next");
            if (issueFile == null)
                return null;
            return new IssueFileDto
            {
                Id = issueFile.Id,
                Code = issueFile.Code,
                IssueNumber = issueFile.IssueNumber,
                IssueName = issueFile.IssueName,
                DateNow = issueFile.DateNow,
                ClientId = issueFile.ClientId,
                ClientProperty = issueFile.ClientProperty,
                ClientPhoneNumber = issueFile.ClientPhoneNumber,
                ClientAddress = issueFile.ClientAddress,
                PartyId = issueFile.PartyId,
                PartyPhoneNumber = issueFile.PartyPhoneNumber,
                PartyAddress = issueFile.PartyAddress,
                PartyWorkPlace = issueFile.PartyWorkPlace,
                IssueTypeId = issueFile.IssueTypeId,
                IssueValueFees = issueFile.IssueValueFees,
                IssueDescription = issueFile.IssueDescription,
                IssueDegreeNegotiation = issueFile.IssueDegreeNegotiation,
                CourtId = issueFile.CourtId,
                ClaimNumber = issueFile.ClaimNumber,
                YearOfIssue = issueFile.YearOfIssue,
                CenterId = issueFile.CenterId,
                RuleOfIssue = issueFile.RuleOfIssue,
                DateNextSession = issueFile.DateNextSession,
                WhatHappenedInTheCourtSession = issueFile.WhatHappenedInTheCourtSession,
                ClientImageBase64 = issueFile.IssueImage != null ? Convert.ToBase64String(issueFile.IssueImage) : null,

            };
        }

        public async Task<IssueFileDto> GetPreviousIssueFile(int id)
        {
            var issueFile = await _unitOfWork.issuesFile.GetNextOrPreviousItemByCode(id, "previous");
            if (issueFile == null)
                return null;
            return new IssueFileDto
            {
                Id = issueFile.Id,
                Code = issueFile.Code,
                IssueNumber = issueFile.IssueNumber,
                IssueName = issueFile.IssueName,
                DateNow = issueFile.DateNow,
                ClientId = issueFile.ClientId,
                ClientProperty = issueFile.ClientProperty,
                ClientPhoneNumber = issueFile.ClientPhoneNumber,
                ClientAddress = issueFile.ClientAddress,
                PartyId = issueFile.PartyId,
                PartyPhoneNumber = issueFile.PartyPhoneNumber,
                PartyAddress = issueFile.PartyAddress,
                PartyWorkPlace = issueFile.PartyWorkPlace,
                IssueTypeId = issueFile.IssueTypeId,
                IssueValueFees = issueFile.IssueValueFees,
                IssueDescription = issueFile.IssueDescription,
                IssueDegreeNegotiation = issueFile.IssueDegreeNegotiation,
                CourtId = issueFile.CourtId,
                ClaimNumber = issueFile.ClaimNumber,
                YearOfIssue = issueFile.YearOfIssue,
                CenterId = issueFile.CenterId,
                RuleOfIssue = issueFile.RuleOfIssue,
                DateNextSession = issueFile.DateNextSession,
                WhatHappenedInTheCourtSession = issueFile.WhatHappenedInTheCourtSession,
                ClientImageBase64 = issueFile.IssueImage != null ? Convert.ToBase64String(issueFile.IssueImage) : null,

            };
        }
    }
}
