using Microsoft.AspNetCore.Identity;
using Serilog;
using SpaceXMission.Entities;
using SpaceXMission_Repository.Interfaces;

namespace SpaceXMission.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _userManager.FindByEmailAsync(email);
            }
            // rethrows the caught exception. It allows the exception to propagate further up to the call stack (service layer) and higher levels have a chance to
            // handle it (ex. display an error message to the user)
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            try
            {
                return await _userManager.FindByNameAsync(username);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        public async Task<string> GetUserIdByUsernameAsync(string username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                return user?.Id;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            try
            {
                return await _userManager.CreateAsync(user, password);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            try
            {
                return await _userManager.CheckPasswordAsync(user, password);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
    }
}
