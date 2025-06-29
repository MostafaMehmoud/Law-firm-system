using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Law.CORE.ViewModels;

namespace Law.BL.Services.IServices
{
    public interface IAuthService
    {
        Task<OperationResult> RegisterAsync(VWUser vWUser);
        Task<OperationResult> LoginAsync(string username, string password);
        Task<OperationResult> UpdateUserAsync(VWUser vWUser);
        Task<OperationResult> DeleteUserAsync(string id);
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<ApplicationUser> GetMin();
        Task<ApplicationUser> GetMax();
        Task<ApplicationUser> GetNextUser(int id);
        Task<ApplicationUser> GetPreviousUser(int id);
        Task<int> GetMaxIdOfItem();
        Task<int> GetNewCode();
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        Task Logout();
        Task<string> GenerateResetPasswordLinkAsync(string username, HttpRequest request, IUrlHelper urlHelper);
        Task<IdentityResult> ResetPasswordAsync(string username, string token, string newPassword);
        Task<string> AuthenticateUser(string username, string password);
        Task<bool> RegisterUser(ApplicationUser user, string password);
        public  Task<OperationResult> RegisterWithoutRolesAsync(VWUser vWUser);
    }
}
