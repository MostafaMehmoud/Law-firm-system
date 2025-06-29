using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.ViewModels;

namespace Law.BL.Services.IServices
{
    public interface IOfferService
    {
        Task<bool> HasUserSubmittedOfferAsync(string userId, int caseId);
        Task<bool> SubmitOfferAsync(SubmitOfferViewModel model, string userId);
        Task<SubmitOfferViewModel> GetOfferForEditAsync(string userId, int caseId);
        Task<bool> UpdateOfferAsync(SubmitOfferViewModel model, string userId);
        Task<List<UserOfferViewModel>> GetOffersByUserAsync(string userId);


    }

}
