using SpaceXMission.Dtos;

namespace SpaceXMission_Service.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> Register(RegisterDto request);
        Task<string> Login(LoginDto request);
    }
}
