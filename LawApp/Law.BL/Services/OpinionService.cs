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
    public class OpinionService : IOpinionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OpinionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddOpinionAsync(AddOpinionViewModel model, string userId)
        {
            var opinion = new Opinion
            {
                CaseId = model.CaseId,
                UserId = userId,
                Comment = model.Comment,
                PostedAt = DateTime.Now
            };

            await _unitOfWork.OpinionRepository.Add(opinion);
             _unitOfWork.Complete();

            return true;
        }
        public async Task<List<OpinionDisplayViewModel>> GetAllOpinionsForAdminAsync()
        {
            var opinions = await _unitOfWork.OpinionRepository
                .GetAllWithUserAndCaseAsync(); // سننشئها الآن في DAL

            return opinions.Select(o => new OpinionDisplayViewModel
            {
                CaseTitle = o.Case.Title,
                LawyerName = o.User.UserName, // أو UserName حسب التطبيق
                Comment = o.Comment,
                PostedAt = o.PostedAt
            }).ToList();
        }
        public async Task<List<OpinionDisplayViewModel>> GetUserOpinionsAsync(string userId)
        {
            var opinions = await _unitOfWork.OpinionRepository
                .GetOpinionsByUserIdAsync(userId);

            return opinions.Select(o => new OpinionDisplayViewModel
            {
                CaseTitle = o.Case.Title,
                LawyerName = o.User.UserName,
                Comment = o.Comment,
                PostedAt = o.PostedAt
            }).ToList();
        }
        public async Task<AddOpinionViewModel> GetOpinionForEditAsync(int opinionId, string userId)
        {
            var opinion = await _unitOfWork.OpinionRepository.GetById(opinionId);
            if (opinion == null || opinion.UserId != userId)
                return null;

            return new AddOpinionViewModel
            {
                Id = opinion.Id,
                CaseId = opinion.CaseId,
                Comment = opinion.Comment
            };
        }

        public async Task<bool> UpdateOpinionAsync(AddOpinionViewModel model, string userId)
        {
            var opinion = await _unitOfWork.OpinionRepository.GetById(model.Id);
            if (opinion == null || opinion.UserId != userId)
                return false;

            opinion.Comment = model.Comment;
            opinion.PostedAt = DateTime.Now;

             _unitOfWork.Complete();
            return true;
        }
        public async Task<bool> DeleteOpinionAsync(int id, string userId)
        {
            var opinion = await _unitOfWork.OpinionRepository.GetById(id);
            if (opinion == null || opinion.UserId != userId)
                return false;

            return await _unitOfWork.OpinionRepository.Delete(id);
        }
        public async Task<List<OpinionDisplayViewModel>> GetOpinionsByCaseIdAsync(int caseId)
        {
            var opinions = await _unitOfWork.OpinionRepository.GetOpinionsByCaseIdAsync(caseId);

            return opinions.Select(o => new OpinionDisplayViewModel
            {
                Id=o.Id,
                CaseTitle = o.Case.Title,
                LawyerName = o.User.UserName,
                Comment = o.Comment,
                PostedAt = o.PostedAt
            }).ToList();
        }
        public async Task<bool> HasUserSubmittedOpinionAsync(string userId, int caseId)
        {
            return await _unitOfWork.OpinionRepository
                .ExistsAsync(o => o.UserId == userId && o.CaseId == caseId);
        }
        public async Task<OpinionEditViewModel> GetForEditAsync(int id)
        {
            var opinion = await _unitOfWork.OpinionRepository.GetByIdAsync(id);
            if (opinion == null)
                return null;

            return new OpinionEditViewModel
            {
                Id = opinion.Id,
                CaseId = opinion.CaseId,
                Comment = opinion.Comment
            };
        }

        public async Task<bool> UpdateOpinionAsync(OpinionEditViewModel model, string userId)
        {
            var opinion = await _unitOfWork.OpinionRepository.GetByIdAsync(model.Id);
            if (opinion == null || opinion.UserId != userId)
                return false;

            opinion.Comment = model.Comment;
             _unitOfWork.Complete();
            return true;
        }


    }

}
