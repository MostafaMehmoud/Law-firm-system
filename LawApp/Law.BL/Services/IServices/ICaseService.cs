using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Law.CORE.ViewModels;

namespace Law.BL.Services.IServices
{
    public interface ICaseService
    {
        Task<List<CaseViewModel>> GetCasesWithOfferStatusAsync(string userId);
        Task<Case> GetCaseByIdAsync(int id);
        Task<bool> AddCaseAsync(CaseViewModel model);
        public Task<List<CaseWithOfferStatusViewModel>> GetCasesWithOfferStatusForUserAsync(string userId);
        Task<CaseDetailViewModel> GetCaseWithOpinionsAsync(int caseId);
        public Task<bool> DeleteCaseAsync(int id);
    }

}
