using SpaceXMission_Domain.Dtos;
using SpaceXMission_Shared.Enums;
using SpaceXMission_Shared.Helpers.Models;

namespace SpaceXMission_Service.Interfaces
{
    public interface ISpaceXMissionService
    {
        Task<ApiResponse<SpaceXMissionResponseModel>> GetLatestDataFromSpaceXMissionsApi();
        Task<ApiResponse<List<SpaceXMissionResponseModel>>> GetLaunchesFromSpaceCMissionsApi(LaunchType launchType);
    }
}
