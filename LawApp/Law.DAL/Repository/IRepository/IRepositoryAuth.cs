using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Law.CORE.ViewModels;

namespace Law.DAL.Repository.IRepository
{
    public interface IRepositoryAuth
    {
        public Task<OperationResult> Register(ApplicationUser user, string password);
        Task<OperationResult> UpdateUser(ApplicationUser user);
        Task<OperationResult> DeleteUser(ApplicationUser user);
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> GetUserById(string id);
        Task<ApplicationUser> GetMin();
        Task<ApplicationUser> GetMax();
        Task<ApplicationUser> GetNextOrPreviousItemByCode(int id, string direction);
        int GetMaxIdOfItem();
        Task<int> GetNewCode();
        public Task<OperationResult> Login(string username, string password);
        Task Logout();
        Task<ApplicationUser> GetUserByUsernameAsync(string username);
        Task<bool> ValidateUserCredentials(string username, string password);
        Task<bool> CreateUser(ApplicationUser user, string password);
    }
}
