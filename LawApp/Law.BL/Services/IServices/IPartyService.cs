using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Law.CORE.ViewModels;

namespace Law.BL.Services.IServices
{
    public interface IPartyService
    {
        public Task<string> Add(UpdateParty model);
        public Task<string> Edit(UpdateParty model);
        public Task<string> Delete(int id);
        public Task<Party> GetbyId(int id);
        public Task<IEnumerable<Party>> GetAll();
        public Task<int> GetNewCode();
        Task<PartyDto> GetNextParty(int id);
        Task<PartyDto> GetPreviousParty(int id);
        Task<PartyDto> GetMinParty();
        Task<PartyDto> GetMaxParty();
        int GetMaxPartyId();
    }
}
