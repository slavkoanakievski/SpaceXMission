using SpaceXMission.Dtos;
using SpaceXMission_Domain.Dtos;
using SpaceXMission_Shared.Helpers.Models;

namespace SpaceXMission_Service.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<string>> Register(RegisterDto request);
        Task<ApiResponse<AuthenticatedResponse>> Login(LoginDto request);
    }
}
