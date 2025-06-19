using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.BL.Services.IServices;
using Law.CORE.Entities;
using Law.CORE.ViewModels;
using Law.DAL.Migrations;
using Law.DAL.Repository.IRepository;

namespace Law.BL.Services
{
    public class CourtService : ICourtService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CourtService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Add(CreateCourt model)
        {
            Court court = new Court()
            {
                Code = model.Code,
                CourtName=model.CourtName,
                JudgeName=model.JudgeName,
                PhoneNumber=model.PhoneNumber,
                FaxNumber=model.FaxNumber
            };
            if (await _unitOfWork.courts.Add(court))
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
            if (await _unitOfWork.courts.Delete(id))
            {
                return "تم الحذف بنجاح";
            }
            else
            {
                return "حدثت مشكلة اثناء الحذف";
            }
        }

        public async Task<string> Edit(UpdateCourt model)
        {
            Court court = new Court()
            {
                Id=model.Id,
                Code = model.Code,
                CourtName = model.CourtName,
                JudgeName = model.JudgeName,
                PhoneNumber = model.PhoneNumber,
                FaxNumber = model.FaxNumber
            };
            if (await _unitOfWork.courts.Update(court))
            {
                return "تم التعديل بنجاح";
            }
            else
            {
                return "حدثت مشكلة أثناء التعديل";
            }
        }

        public async Task<IEnumerable<Court>> GetAll()
        {
            return await _unitOfWork.courts.GetAll();
        }

        public async Task<Court> GetbyId(int id)
        {
            return await _unitOfWork.courts.GetById(id);
        }

        public async Task<UpdateCourt> GetMaxCourt()
        {
            var court = await _unitOfWork.courts.GetMax();
            UpdateCourt UpdateCourt = new UpdateCourt()
            {
                Id = court.Id,
                Code = court.Code,
                CourtName = court.CourtName,
                JudgeName= court.JudgeName,
                FaxNumber= court.FaxNumber,
                PhoneNumber = court.PhoneNumber,    
            };
            return UpdateCourt;
        }

        public int GetMaxCourtId()
        {
            return _unitOfWork.courts.GetMaxIdOfItem();
        }

        public async Task<UpdateCourt> GetMinCourt()
        {
            var court = await _unitOfWork.courts.GetMin();
            UpdateCourt UpdateCourt = new UpdateCourt()
            {
                Id = court.Id,
                Code = court.Code,
                CourtName = court.CourtName,
                JudgeName = court.JudgeName,
                FaxNumber = court.FaxNumber,
                PhoneNumber = court.PhoneNumber,
            };
            return UpdateCourt;
        }

        public async Task<int> GetNewCode()
        {
            return await _unitOfWork.courts.GetNewCode();
        }

        public async Task<UpdateCourt> GetNextCourt(int id)
        {
            var court = await _unitOfWork.courts.GetNextOrPreviousItemByCode(id, "next");
            if (court == null)
            {
                return null;
            }
            UpdateCourt UpdateCourt = new UpdateCourt()
            {
                Id = court.Id,
                Code = court.Code,
                CourtName = court.CourtName,
                JudgeName = court.JudgeName,
                FaxNumber = court.FaxNumber,
                PhoneNumber = court.PhoneNumber,
            };
            return UpdateCourt;
        }

        public async Task<UpdateCourt> GetPreviousCourt(int id)
        {
            var court = await _unitOfWork.courts.GetNextOrPreviousItemByCode(id, "previous");
            if (court == null)
            {
                return null;
            }
            UpdateCourt UpdateCourt = new UpdateCourt()
            {
                Id = court.Id,
                Code = court.Code,
                CourtName = court.CourtName,
                JudgeName = court.JudgeName,
                FaxNumber = court.FaxNumber,
                PhoneNumber = court.PhoneNumber,
            };
            return UpdateCourt;
        }
    }
}
