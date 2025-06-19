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
    public class IssueService : IIssueService
    {
        private readonly IUnitOfWork _unitOfWork;
        public IssueService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Add(CreateIssue model)
        {
            Issue issue = new Issue()
            {
                Code=model.Code,
                Name=model.Name,    
            };
            if (await _unitOfWork.issues.Add(issue))
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
           
            if (await _unitOfWork.issues.Delete(id))
            {
                return "تم الحذف بنجاح";
            }
            else
            {
                return "حدثت مشكلة اثناء الحذف";
            }
        }

        public async Task<string> Edit(UpdateIssue model)
        {
            Issue issue = new Issue()
            {
                Id= model.Id,  
                Code = model.Code,
                Name = model.Name,
            };
            if (await _unitOfWork.issues.Update(issue))
            {
                return "تم التعديل بنجاح";
            }
            else
            {
                return "حدثت مشكلة أثناء التعديل";
            }
        }

        public async Task<IEnumerable<Issue>> GetAll()
        {
            return await _unitOfWork.issues.GetAll();
        }

        public async Task<Issue> GetbyId(int id)
        {
            return await _unitOfWork.issues.GetById(id);
        }

        public async Task<UpdateIssue> GetMaxIssue()
        {
            var issue = await _unitOfWork.issues.GetMax();
            UpdateIssue updateIssue = new UpdateIssue()
            {
                Id = issue.Id,
                Code = issue.Code,
                Name = issue.Name
            };
            return updateIssue; 
        }

        public int GetMaxIssueId()
        {
            return _unitOfWork.issues.GetMaxIdOfItem();
        }

        public async Task<UpdateIssue> GetMinIssue()
        {
            var issue = await _unitOfWork.issues.GetMin();
            UpdateIssue updateIssue = new UpdateIssue()
            {
                Id = issue.Id,
                Code = issue.Code,
                Name = issue.Name
            };
            return updateIssue;
        }

        public async Task<int> GetNewCode()
        {
            return await _unitOfWork.issues.GetNewCode();
        }

        public async Task<UpdateIssue> GetNextIssue(int id)
        {
            var issue = await _unitOfWork.issues.GetNextOrPreviousItemByCode(id, "next");
            if (issue == null)
            {
                return null;
            }
            UpdateIssue updateIssue = new UpdateIssue()
            {
                Id = issue.Id,
                Code = issue.Code,
                Name = issue.Name
            };
            return updateIssue;
        }

        public async Task<UpdateIssue> GetPreviousIssue(int id)
        {
            var issue = await _unitOfWork.issues.GetNextOrPreviousItemByCode(id, "previous");
            if (issue == null)
            {
                return null;
            }
            UpdateIssue updateIssue = new UpdateIssue()
            {
                Id = issue.Id,
                Code = issue.Code,
                Name = issue.Name
            };
            return updateIssue;
        }
    }
}
