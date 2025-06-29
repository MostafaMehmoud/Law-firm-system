using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Law.CORE.ViewModels;

namespace Law.BL.Services.IServices
{
    public interface ICourtSessionService
    {
        public Task<string> Add(UpdateCourtSession model);
        public Task<string> Edit(UpdateCourtSession model);
        public Task<string> Delete(int id);
        public Task<CourtSession> GetbyId(int id);
        public Task<IEnumerable<CourtSession>> GetAll();
        public Task<int> GetNewCode();
        Task<CourtSessionDto> GetNextCourtSession(int id);
        Task<CourtSessionDto> GetPreviousCourtSession(int id);
        Task<CourtSessionDto> GetMinCourtSession();
        Task<CourtSessionDto> GetMaxCourtSession();
        int GetMaxCourtSessionId();
        

    }
}
