using SpaceXMission.Dtos;

namespace SpaceXMission.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> Register(RegisterDto request);
        Task<string> Login(LoginDto request);

    }
}
