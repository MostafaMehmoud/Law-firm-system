using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Law.CORE.ViewModels;

namespace Law.BL.Services.IServices
{
    public interface IOpinionService
    {
        Task<bool> AddOpinionAsync(AddOpinionViewModel model, string userId);
        Task<List<OpinionDisplayViewModel>> GetAllOpinionsForAdminAsync();
        Task<List<OpinionDisplayViewModel>> GetUserOpinionsAsync(string userId);
        Task<AddOpinionViewModel> GetOpinionForEditAsync(int opinionId, string userId);
        Task<bool> UpdateOpinionAsync(AddOpinionViewModel model, string userId);
        Task<bool> DeleteOpinionAsync(int id, string userId);
        Task<List<OpinionDisplayViewModel>> GetOpinionsByCaseIdAsync(int caseId);
        Task<bool> HasUserSubmittedOpinionAsync(string userId, int caseId);
        Task<OpinionEditViewModel> GetForEditAsync(int id);
        Task<bool> UpdateOpinionAsync(OpinionEditViewModel model, string userId);




    }

}
