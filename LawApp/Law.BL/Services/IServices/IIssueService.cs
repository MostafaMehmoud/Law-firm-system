using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Law.CORE.ViewModels;

namespace Law.BL.Services.IServices
{
    public interface IIssueService
    {
        public Task<string> Add(CreateIssue model);
        public Task<string> Edit(UpdateIssue model);
        public Task<string> Delete(int id);
        public Task<Issue> GetbyId(int id);
        public Task<IEnumerable<Issue>> GetAll();
        public Task<int> GetNewCode();
        Task<UpdateIssue> GetNextIssue(int id);
        Task<UpdateIssue> GetPreviousIssue(int id);
        Task<UpdateIssue> GetMinIssue();
        Task<UpdateIssue> GetMaxIssue();
        int GetMaxIssueId();
    }
}
