using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Law.CORE.ViewModels;

namespace Law.BL.Services.IServices
{
    public interface ICourtService
    {
        public Task<string> Add(CreateCourt model);
        public Task<string> Edit(UpdateCourt model);
        public Task<string> Delete(int id);
        public Task<Court> GetbyId(int id);
        public Task<IEnumerable<Court>> GetAll();
        public Task<int> GetNewCode();
        Task<UpdateCourt> GetNextCourt(int id);
        Task<UpdateCourt> GetPreviousCourt(int id);
        Task<UpdateCourt> GetMinCourt();
        Task<UpdateCourt> GetMaxCourt();
        int GetMaxCourtId();
    }
}
