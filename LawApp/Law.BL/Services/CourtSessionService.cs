using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Law.BL.Services.IServices;
using Law.CORE.Entities;
using Law.CORE.ViewModels;
using Law.DAL.Repository.IRepository;

namespace Law.BL.Services
{
    public class CourtSessionService : ICourtSessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CourtSessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Add(UpdateCourtSession model)
        {
            byte[] imageBytes = Array.Empty<byte>(); // تعيين قيمة مبدئية لتجنب الخطأ
            if (model.IssueImage != null && model.IssueImage.Length > 0)
            {
                using var ms = new MemoryStream();
                await model.IssueImage.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }
            else if (!string.IsNullOrEmpty(model.OldImageBase64))
            {
                var base64Data = Regex.Match(model.OldImageBase64, @"data:image\/[a-zA-Z]+;base64,(?<data>.+)").Groups["data"].Value;
                imageBytes = Convert.FromBase64String(base64Data);
            }

            //byte[] imageBytes;
            //using (var ms = new MemoryStream())
            //{
            //    await model.IssueImage.CopyToAsync(ms);
            //    imageBytes = ms.ToArray();
            //}
            CourtSession courtsession = new CourtSession()
            {
                CourtSessionDate=model.CourtSessionDate,
                IssueFileId=model.IssueFileId,  
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
            if (await _unitOfWork.courtsSession.Add(courtsession))
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
            if (await _unitOfWork.courtsSession.Delete(id))
            {
                return "تم الحذف بنجاح";
            }
            else
            {
                return "حدثت مشكلة اثناء الحذف";
            }
        }

        public async Task<string> Edit(UpdateCourtSession model)
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
                    var base64Data = Regex.Match(model.OldImageBase64, @"data:image\/[a-zA-Z]+;base64,(?<data>.+)").Groups["data"].Value;
                    imageBytes = Convert.FromBase64String(base64Data);
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

            CourtSession courtsession = new CourtSession()
            {
                CourtSessionDate = model.CourtSessionDate,
                IssueFileId = model.IssueFileId,
                Code = model.Code,
                Id=model.Id,
                
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

            if (await _unitOfWork.courtsSession.Update(courtsession))
            {
                return "تم التعديل بنجاح";
            }
            else
            {
                return "حدثت مشكلة أثناء التعديل";
            }
        }

        public async Task<IEnumerable<CourtSession>> GetAll()
        {
            return await _unitOfWork.courtsSession.GetAll();
        }

        public async Task<CourtSession> GetbyId(int id)
        {
            return await _unitOfWork.courtsSession.GetById(id);
        }

        public async Task<CourtSessionDto> GetMaxCourtSession()
        {
            var courtsession = await _unitOfWork.courtsSession.GetMax();
            if (courtsession == null)
                return null;
            return new CourtSessionDto
            {
                CourtSessionDate= courtsession.CourtSessionDate,
                IssueFileId= courtsession.IssueFileId,  

                Id = courtsession.Id,
                Code = courtsession.Code,
                IssueNumber = courtsession.IssueNumber,
                IssueName = courtsession.IssueName,
                DateNow = courtsession.DateNow,
                ClientId = courtsession.ClientId,
                ClientProperty = courtsession.ClientProperty,
                ClientPhoneNumber = courtsession.ClientPhoneNumber,
                ClientAddress = courtsession.ClientAddress,
                PartyId = courtsession.PartyId,
                PartyPhoneNumber = courtsession.PartyPhoneNumber,
                PartyAddress = courtsession.PartyAddress,
                PartyWorkPlace = courtsession.PartyWorkPlace,
                IssueTypeId = courtsession.IssueTypeId,
                IssueValueFees = courtsession.IssueValueFees,
                IssueDescription = courtsession.IssueDescription,
                IssueDegreeNegotiation = courtsession.IssueDegreeNegotiation,
                CourtId = courtsession.CourtId,
                ClaimNumber = courtsession.ClaimNumber,
                YearOfIssue = courtsession.YearOfIssue,
                CenterId = courtsession.CenterId,
                RuleOfIssue = courtsession.RuleOfIssue,
                DateNextSession = courtsession.DateNextSession,
                WhatHappenedInTheCourtSession = courtsession.WhatHappenedInTheCourtSession,
                OldImageBase64 = courtsession.IssueImage != null ? Convert.ToBase64String(courtsession.IssueImage) : null,
                IssueImage = courtsession.IssueImage != null
    ? $"data:image/jpeg;base64,{Convert.ToBase64String(courtsession.IssueImage)}"
    : null,

            };
        }

        public int GetMaxCourtSessionId()
        {
            return _unitOfWork.courtsSession.GetMaxIdOfItem();
        }

        public async Task<CourtSessionDto> GetMinCourtSession()
        {
            var courtsession = await _unitOfWork.courtsSession.GetMin();
            if (courtsession == null)
                return null;
            return new CourtSessionDto
            {
                CourtSessionDate = courtsession.CourtSessionDate,
                IssueFileId = courtsession.IssueFileId,

                Id = courtsession.Id,
                Code = courtsession.Code,
                IssueNumber = courtsession.IssueNumber,
                IssueName = courtsession.IssueName,
                DateNow = courtsession.DateNow,
                ClientId = courtsession.ClientId,
                ClientProperty = courtsession.ClientProperty,
                ClientPhoneNumber = courtsession.ClientPhoneNumber,
                ClientAddress = courtsession.ClientAddress,
                PartyId = courtsession.PartyId,
                PartyPhoneNumber = courtsession.PartyPhoneNumber,
                PartyAddress = courtsession.PartyAddress,
                PartyWorkPlace = courtsession.PartyWorkPlace,
                IssueTypeId = courtsession.IssueTypeId,
                IssueValueFees = courtsession.IssueValueFees,
                IssueDescription = courtsession.IssueDescription,
                IssueDegreeNegotiation = courtsession.IssueDegreeNegotiation,
                CourtId = courtsession.CourtId,
                ClaimNumber = courtsession.ClaimNumber,
                YearOfIssue = courtsession.YearOfIssue,
                CenterId = courtsession.CenterId,
                RuleOfIssue = courtsession.RuleOfIssue,
                DateNextSession = courtsession.DateNextSession,
                WhatHappenedInTheCourtSession = courtsession.WhatHappenedInTheCourtSession,
                OldImageBase64 = courtsession.IssueImage != null ? Convert.ToBase64String(courtsession.IssueImage) : null,
                IssueImage = courtsession.IssueImage != null
    ? $"data:image/jpeg;base64,{Convert.ToBase64String(courtsession.IssueImage)}"
    : null,

            };
        }

        public async Task<int> GetNewCode()
        {
            return await _unitOfWork.courtsSession.GetNewCode();
        }

        public async Task<CourtSessionDto> GetNextCourtSession(int id)
        {
            var courtsession = await _unitOfWork.courtsSession.GetNextOrPreviousItemByCode(id, "next");
            if (courtsession == null)
                return null;
            return new CourtSessionDto
            {
                CourtSessionDate = courtsession.CourtSessionDate,
                IssueFileId = courtsession.IssueFileId,

                Id = courtsession.Id,
                Code = courtsession.Code,
                IssueNumber = courtsession.IssueNumber,
                IssueName = courtsession.IssueName,
                DateNow = courtsession.DateNow,
                ClientId = courtsession.ClientId,
                ClientProperty = courtsession.ClientProperty,
                ClientPhoneNumber = courtsession.ClientPhoneNumber,
                ClientAddress = courtsession.ClientAddress,
                PartyId = courtsession.PartyId,
                PartyPhoneNumber = courtsession.PartyPhoneNumber,
                PartyAddress = courtsession.PartyAddress,
                PartyWorkPlace = courtsession.PartyWorkPlace,
                IssueTypeId = courtsession.IssueTypeId,
                IssueValueFees = courtsession.IssueValueFees,
                IssueDescription = courtsession.IssueDescription,
                IssueDegreeNegotiation = courtsession.IssueDegreeNegotiation,
                CourtId = courtsession.CourtId,
                ClaimNumber = courtsession.ClaimNumber,
                YearOfIssue = courtsession.YearOfIssue,
                CenterId = courtsession.CenterId,
                RuleOfIssue = courtsession.RuleOfIssue,
                DateNextSession = courtsession.DateNextSession,
                WhatHappenedInTheCourtSession = courtsession.WhatHappenedInTheCourtSession,
                OldImageBase64 = courtsession.IssueImage != null ? Convert.ToBase64String(courtsession.IssueImage) : null,
                IssueImage = courtsession.IssueImage != null
    ? $"data:image/jpeg;base64,{Convert.ToBase64String(courtsession.IssueImage)}"
    : null,

            };
        }

        public async Task<CourtSessionDto> GetPreviousCourtSession(int id)
        {
            var courtsession = await _unitOfWork.courtsSession.GetNextOrPreviousItemByCode(id, "previous");
            if (courtsession == null)
                return null;
            return new CourtSessionDto
            {
                CourtSessionDate = courtsession.CourtSessionDate,
                IssueFileId = courtsession.IssueFileId,

                Id = courtsession.Id,
                Code = courtsession.Code,
                IssueNumber = courtsession.IssueNumber,
                IssueName = courtsession.IssueName,
                DateNow = courtsession.DateNow,
                ClientId = courtsession.ClientId,
                ClientProperty = courtsession.ClientProperty,
                ClientPhoneNumber = courtsession.ClientPhoneNumber,
                ClientAddress = courtsession.ClientAddress,
                PartyId = courtsession.PartyId,
                PartyPhoneNumber = courtsession.PartyPhoneNumber,
                PartyAddress = courtsession.PartyAddress,
                PartyWorkPlace = courtsession.PartyWorkPlace,
                IssueTypeId = courtsession.IssueTypeId,
                IssueValueFees = courtsession.IssueValueFees,
                IssueDescription = courtsession.IssueDescription,
                IssueDegreeNegotiation = courtsession.IssueDegreeNegotiation,
                CourtId = courtsession.CourtId,
                ClaimNumber = courtsession.ClaimNumber,
                YearOfIssue = courtsession.YearOfIssue,
                CenterId = courtsession.CenterId,
                RuleOfIssue = courtsession.RuleOfIssue,
                DateNextSession = courtsession.DateNextSession,
                WhatHappenedInTheCourtSession = courtsession.WhatHappenedInTheCourtSession,
                OldImageBase64 = courtsession.IssueImage != null ? Convert.ToBase64String(courtsession.IssueImage) : null,
                IssueImage = courtsession.IssueImage != null
    ? $"data:image/jpeg;base64,{Convert.ToBase64String(courtsession.IssueImage)}"
    : null,

            };
        }
    }
}
