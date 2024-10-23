using SpaceXMission.Dtos;
using SpaceXMission_Shared.Helpers.Models;

namespace SpaceXMission_Service.Interfaces
{
    public interface IValidationService
    {
        Task<ApiResponse<bool>> ValidateLoginFields(LoginDto loginModel);
        Task<ApiResponse<bool>> ValidateFieldsForRegisterAccount(RegisterDto registerDto);

    }
}
