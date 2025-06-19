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
    public class CenterService : ICenterService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CenterService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Add(CreateCenter model)
        {
            Center center = new Center()
            {
                Code = model.Code,
                Name = model.Name,  
                CourtId = model.CourtId,
            };
            if (await _unitOfWork.centers.Add(center))
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
            if (await _unitOfWork.centers.Delete(id))
            {
                return "تم الحذف بنجاح";
            }
            else
            {
                return "حدثت مشكلة اثناء الحذف";
            }
        }

        public async Task<string> Edit(UpdateCenter model)
        {
            Center center = new Center()
            {
                Id=model.Id,
                Code = model.Code,
                Name = model.Name,
                CourtId = model.CourtId,
            }; if (await _unitOfWork.centers.Update(center))
            {
                return "تم التعديل بنجاح";
            }
            else
            {
                return "حدثت مشكلة أثناء التعديل";
            }
        }

        public async Task<IEnumerable<Center>> GetAll()
        {
            return await _unitOfWork.centers.GetAll();
        }

        public async Task<Center> GetbyId(int id)
        {
            return await _unitOfWork.centers.GetById(id);
        }

        public async Task<UpdateCenter> GetMaxCenter()
        {
            var center = await _unitOfWork.centers.GetMax();
            UpdateCenter updateCenter = new UpdateCenter()
            {
                Id = center.Id,
                Code = center.Code,
                Name = center.Name,
                CourtId = center.CourtId,
            };
            return updateCenter;
        }

        public int GetMaxCenterId()
        {
            return _unitOfWork.centers.GetMaxIdOfItem();
        }

        public async Task<UpdateCenter> GetMinCenter()
        {
            var center = await _unitOfWork.centers.GetMin();
            UpdateCenter updateCenter = new UpdateCenter()
            {
                Id = center.Id,
                Code = center.Code,
                Name = center.Name,
                CourtId = center.CourtId,
            };
            return updateCenter;
        }

        public async Task<int> GetNewCode()
        {
            return await _unitOfWork.centers.GetNewCode();
        }

        public async Task<UpdateCenter> GetNextCenter(int id)
        {
            var center = await _unitOfWork.centers.GetNextOrPreviousItemByCode(id, "next");
            if (center == null)
            {
                return null;
            }
            UpdateCenter updateCenter = new UpdateCenter()
            {
                Id = center.Id,
                Code = center.Code,
                Name = center.Name,
                CourtId = center.CourtId,
            };
            return updateCenter;
        }

        public async Task<UpdateCenter> GetPreviousCenter(int id)
        {
            var center = await _unitOfWork.centers.GetNextOrPreviousItemByCode(id, "previous");
            if (center == null)
            {
                return null;
            }
            UpdateCenter updateCenter = new UpdateCenter()
            {
                Id = center.Id,
                Code = center.Code,
                Name = center.Name,
                CourtId = center.CourtId,
            };
            return updateCenter;
        }
    }
}
