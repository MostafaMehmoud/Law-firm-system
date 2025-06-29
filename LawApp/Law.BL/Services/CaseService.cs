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
    public class CaseService : ICaseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> AddCaseAsync(CaseViewModel model)
        {
            var entity = new Case
            {
                Title = model.Title,
                Description = model.Description,
                CreatedAt = model.CreatedAt
            };

            await _unitOfWork.cases.Add(entity);
             _unitOfWork.Complete();

            return true;
        }

        public async Task<Case> GetCaseByIdAsync(int id)
        {
            var entity = await _unitOfWork.cases.GetByIdWithIncludesAsync(id);
            if (entity == null)
                return null;

            return new Case
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
              
                CreatedAt = entity.CreatedAt,
                // أي خصائص إضافية تحتاجها
            };
        }

        public async Task<List<CaseViewModel>> GetCasesWithOfferStatusAsync(string userId)
        {
            var cases = await _unitOfWork.cases.GetAllWithOffersAsync();

            var result = cases.Select(c => new CaseViewModel
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                HasSubmittedOffer = c.Offers.Any(o => o.UserId == userId)
            }).ToList();

            return result;
        }
        public async Task<List<CaseWithOfferStatusViewModel>> GetCasesWithOfferStatusForUserAsync(string userId)
        {
            var allCases = await _unitOfWork.cases.GetAll();

            var userOffers = (await _unitOfWork.OfferRepository.GetAll())
                .Where(o => o.UserId == userId)
                .ToList();

            var result = allCases.Select(c => new CaseWithOfferStatusViewModel
            {
                CaseId = c.Id,
                Title = c.Title,
                CreatedAt = c.CreatedAt,
                HasUserSubmittedOffer = userOffers.Any(o => o.CaseId == c.Id),
                OfferId = userOffers.FirstOrDefault(o => o.CaseId == c.Id)?.Id
            }).ToList();

            return result;
        }
        public async Task<CaseDetailViewModel> GetCaseWithOpinionsAsync(int caseId)
        {
            var caseEntity = await _unitOfWork.cases.GetByIdWithOpinionsAsync(caseId);

            if (caseEntity == null)
                return null;

            return new CaseDetailViewModel
            {
                Id = caseEntity.Id,
                Title = caseEntity.Title,
                Description = caseEntity.Description,
                CreatedAt = caseEntity.CreatedAt,
                Opinions = caseEntity.Opinions?.Select(o => new OpinionViewModel
                {
                    Id = o.Id,                                   // ✅ أضف هذا
                    UserId = o.UserId,
                    LawyerName = o.User?.UserName ?? o.User?.UserName ?? "مستخدم",
                    Comment = o.Comment,
                    PostedAt = o.PostedAt
                }).OrderByDescending(o => o.PostedAt).ToList()


            };
        }
        public async Task<bool> DeleteCaseAsync(int id)
        {
            var caseEntity = await _unitOfWork.cases.GetById(id);

            if (caseEntity == null)
                return false;

            await _unitOfWork.cases.Delete(id);
            return true;
        }

    }

}
