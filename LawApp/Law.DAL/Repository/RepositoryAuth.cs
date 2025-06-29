using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Law.CORE.ViewModels;
using Law.DAL.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Law.DAL.Repository
{
    public class RepositoryAuth : IRepositoryAuth
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManger;
        public RepositoryAuth(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManger = signInManager;
        }
        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }
        public async Task<bool> ValidateUserCredentials(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user != null && await _userManager.CheckPasswordAsync(user, password);
        }
        public async Task<OperationResult> DeleteUser(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return new OperationResult
                {
                    Success = result.Succeeded,
                    Message = "تم الحذف بنجاح"
                };
            }
            return new OperationResult
            {
                Success = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }


        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return await Task.FromResult(_userManager.Users.ToList());
        }


        public async Task<ApplicationUser> GetMax()
        {
            return await Task.FromResult(_userManager.Users.OrderByDescending(u => u.UserNumber).FirstOrDefault());
        }

        public async Task<ApplicationUser> GetMin()
        {
            return await Task.FromResult(_userManager.Users.OrderBy(u => u.UserNumber).FirstOrDefault());
        }

        public int GetMaxIdOfItem()
        {
            return _userManager.Users.Any() ? _userManager.Users.Max(u => u.UserNumber) : 0;
        }

        public Task<int> GetNewCode()
        {
            var maxCode = GetMaxIdOfItem();
            return Task.FromResult(maxCode + 1);
        }

        public async Task<ApplicationUser> GetNextOrPreviousItemByCode(int id, string direction)
        {
            var users = _userManager.Users.OrderBy(u => u.UserNumber).ToList();

            int index = users.FindIndex(u => u.UserNumber == id);

            if (index == -1) return null;

            if (direction == "next" && index < users.Count - 1)
                return users[index + 1];

            if (direction == "previous" && index > 0)
                return users[index - 1];

            return null;
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }


        public async Task<OperationResult> Register(ApplicationUser user, string password)
        {
            
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return new OperationResult
                {
                    Success = result.Succeeded,
                    Message = "تم الحفظ بنجاح"
                };
            }
            return new OperationResult
            {
                Success = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };

        }


        public async Task<OperationResult> UpdateUser(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new OperationResult
                {
                    Success = result.Succeeded,
                    Message = "تم الحفظ بنجاح"
                };
            }
            return new OperationResult
            {
                Success = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };

        }
        public async Task<OperationResult> Login(string username, string password)
        {
            var result = await _signInManger.PasswordSignInAsync(username, password, false, false);

            var response = new OperationResult
            {
                Success = result.Succeeded
            };

            if (!result.Succeeded)
            {
                response.Errors.Add("اسم المستخدم أو كلمة المرور غير صحيحة.");
            }

            return response;
        }
        public async Task<bool> CreateUser(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }
        public async Task Logout()
        {
            await _signInManger.SignOutAsync();
        }
    }
}
