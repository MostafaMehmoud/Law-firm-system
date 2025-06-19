using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Law.CORE.ViewModels;

namespace Law.BL.Services.IServices
{
    public interface IIssueFileService

    {
        public Task<string> Add(UpdateIssueFile model);
        public Task<string> Edit(UpdateIssueFile model);
        public Task<string> Delete(int id);
        public Task<IssueFile> GetbyId(int id);
        public Task<IEnumerable<IssueFile>> GetAll();
        public Task<int> GetNewCode();
        Task<IssueFileDto> GetNextIssueFile(int id);
        Task<IssueFileDto> GetPreviousIssueFile(int id);
        Task<IssueFileDto> GetMinIssueFile();
        Task<IssueFileDto> GetMaxIssueFile();
        int GetMaxIssueFileId();
    }
}
