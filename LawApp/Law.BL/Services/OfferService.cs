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
    public class OfferService : IOfferService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OfferService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> HasUserSubmittedOfferAsync(string userId, int caseId)
        {
            return await _unitOfWork.OfferRepository.ExistsAsync(o => o.UserId == userId && o.CaseId == caseId);
        }

        public async Task<bool> SubmitOfferAsync(SubmitOfferViewModel model, string userId)
        {
            var exists = await HasUserSubmittedOfferAsync(userId, model.CaseId);
            if (exists) return false;

            var offer = new Offer
            {
                CaseId = model.CaseId,
                UserId = userId,
                LawyerName = model.LawyerName,
                PhoneNumber = model.PhoneNumber,
                OfficeNumber = model.OfficeNumber,
                ProposedSolution = model.ProposedSolution,
                ProposedFees = model.ProposedFees,
                SubmittedAt = DateTime.Now
            };

            await _unitOfWork.OfferRepository.Add(offer);
           
            return true;
        }
        public async Task<SubmitOfferViewModel> GetOfferForEditAsync(string userId, int caseId)
        {
            var offer = await _unitOfWork.OfferRepository.GetOfferByUserAndCaseAsync(userId, caseId);
            if (offer == null) return null;

            return new SubmitOfferViewModel
            {
                CaseId = offer.CaseId,
                LawyerName = offer.LawyerName,
                PhoneNumber = offer.PhoneNumber,
                OfficeNumber = offer.OfficeNumber,
                ProposedSolution = offer.ProposedSolution,
                ProposedFees = offer.ProposedFees
            };
        }

        public async Task<bool> UpdateOfferAsync(SubmitOfferViewModel model, string userId)
        {
            var offer = await _unitOfWork.OfferRepository.GetOfferByUserAndCaseAsync(userId, model.CaseId);
            if (offer == null) return false;

            offer.LawyerName = model.LawyerName;
            offer.PhoneNumber = model.PhoneNumber;
            offer.OfficeNumber = model.OfficeNumber;
            offer.ProposedSolution = model.ProposedSolution;
            offer.ProposedFees = model.ProposedFees;
            offer.SubmittedAt = DateTime.Now;

            _unitOfWork.Complete();
            return true;
        }
        public async Task<List<UserOfferViewModel>> GetOffersByUserAsync(string userId)
        {
            var offers = await _unitOfWork.OfferRepository
                .GetOffersByUserAsync(userId); // DAL

            return offers.Select(o => new UserOfferViewModel
            {
                OfferId = o.Id,
                CaseId = o.CaseId,
                CaseTitle = o.Case?.Title ?? "غير محدد",
                LawyerName = o.LawyerName,
                PhoneNumber = o.PhoneNumber,
                OfficeNumber = o.OfficeNumber,
                ProposedSolution = o.ProposedSolution,
                ProposedFees = o.ProposedFees,
                SubmittedAt = o.SubmittedAt
            }).ToList();
        }


    }

}
