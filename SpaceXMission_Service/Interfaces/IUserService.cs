using Microsoft.AspNetCore.Identity;
using SpaceXMission.Entities;

namespace SpaceXMission_Service.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> UpdateRefreshTokenAsync(ApplicationUser user, string refreshToken);
        Task<ApplicationUser> GetUserByUsernameAsync(string username);
    }
}
