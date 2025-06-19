using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Law.CORE.ViewModels;

namespace Law.BL.Services.IServices
{
    public interface IClientService
    {
        public Task<string> Add(UpdateClient model);
        public Task<string> Edit(UpdateClient model);
        public Task<string> Delete(int id);
        public Task<Client> GetbyId(int id);
        public Task<IEnumerable<Client>> GetAll();
        public Task<int> GetNewCode();
        Task<ClientDto> GetNextClient(int id);
        Task<ClientDto> GetPreviousClient(int id);
        Task<ClientDto> GetMinClient();
        Task<ClientDto> GetMaxClient();
        int GetMaxClientId();
    }
}
