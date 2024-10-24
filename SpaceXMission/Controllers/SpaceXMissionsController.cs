using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SpaceXMission_Domain.Dtos;
using SpaceXMission_Service.Interfaces;
using SpaceXMission_Shared.Constants;
using SpaceXMission_Shared.Enums;
using SpaceXMission_Shared.Helpers.Models;

namespace SpaceXMission.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SpaceXMissionsController : ControllerBase
    {

        private readonly ISpaceXMissionService _spaceXMissionService;

        public SpaceXMissionsController(ISpaceXMissionService spaceXMissionService)
        {
            _spaceXMissionService = spaceXMissionService;
        }

        [HttpGet("latest")]
        public async Task<ApiResponse<SpaceXMissionResponseModel>> Get()
        {
            try
            {
                return await _spaceXMissionService.GetLatestDataFromSpaceXMissionsApi();

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new ApiResponse<SpaceXMissionResponseModel>()
                {
                    Success = false,
                    ErrorMessage = ErrorMessages.GenericErrorControllerMessage
                };
            }
        }

        [HttpGet("upcoming")]
        public async Task<ApiResponse<List<SpaceXMissionResponseModel>>> GetUpcomingLaunches()
        {
            try
            {
                return await _spaceXMissionService.GetLaunchesFromSpaceCMissionsApi(LaunchType.Upcoming);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new ApiResponse<List<SpaceXMissionResponseModel>>()
                {
                    Success = false,
                    ErrorMessage = ErrorMessages.GenericErrorControllerMessage
                };
            }
        }

        [HttpGet("past")]
        public async Task<ApiResponse<List<SpaceXMissionResponseModel>>> GetPastT()
        {
            try
            {
                return await _spaceXMissionService.GetLaunchesFromSpaceCMissionsApi(LaunchType.Past);

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new ApiResponse<List<SpaceXMissionResponseModel>>()
                {
                    Success = false,
                    ErrorMessage = ErrorMessages.GenericErrorControllerMessage
                };
            }
        }
    }
}
