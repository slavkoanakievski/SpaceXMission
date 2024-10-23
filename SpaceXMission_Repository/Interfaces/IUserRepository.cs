using Microsoft.AspNetCore.Identity;
using SpaceXMission.Entities;

namespace SpaceXMission_Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<ApplicationUser> GetUserByUsernameAsync(string username);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    }
}
