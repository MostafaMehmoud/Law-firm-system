using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Law.CORE.ViewModels;

namespace Law.BL.Services.IServices
{
    public interface ICenterService
    {
        public Task<string> Add(CreateCenter model);
        public Task<string> Edit(UpdateCenter model);
        public Task<string> Delete(int id);
        public Task<Center> GetbyId(int id);
        public Task<IEnumerable<Center>> GetAll();
        public Task<int> GetNewCode();
        Task<UpdateCenter> GetNextCenter(int id);
        Task<UpdateCenter> GetPreviousCenter(int id);
        Task<UpdateCenter> GetMinCenter();
        Task<UpdateCenter> GetMaxCenter();
        int GetMaxCenterId();
    }
}
