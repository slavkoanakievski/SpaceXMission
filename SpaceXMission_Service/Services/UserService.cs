using Microsoft.AspNetCore.Identity;
using SpaceXMission.Entities;
using SpaceXMission_Repository.Interfaces;
using SpaceXMission_Service.Interfaces;

namespace SpaceXMission_Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }

        public async Task<IdentityResult> UpdateRefreshTokenAsync(ApplicationUser user, string refreshToken)
        {
            return await _userRepository.UpdateRefreshTokenAsync(user, refreshToken);
        }
    }
}
