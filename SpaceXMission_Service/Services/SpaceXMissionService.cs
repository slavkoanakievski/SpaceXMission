using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SpaceXMission_Domain.Dtos;
using SpaceXMission_Service.Interfaces;
using SpaceXMission_Shared.Constants;
using SpaceXMission_Shared.Enums;
using SpaceXMission_Shared.Helpers.Models;
using System.Net.Http.Headers;

namespace SpaceXMission_Service.Services
{
    public class SpaceXMissionService : ISpaceXMissionService
    {
        private readonly IConfiguration _configuration;

        public SpaceXMissionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<ApiResponse<SpaceXMissionResponseModel>> GetLatestDataFromSpaceXMissionsApi()
        {
            var response = new ApiResponse<SpaceXMissionResponseModel>() { ErrorMessage = "", Success = false };
            string json;

            using (var client = new HttpClient())
            {
                string spaceXMissionsApiPath = _configuration["SpaceXMissionApiConfig:ApiPath"];
                var url = $"{spaceXMissionsApiPath}/latest";

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var endpoint = new Uri(url);
                var result = await client.GetAsync(endpoint);
                json = await result.Content.ReadAsStringAsync();
            }

            SpaceXMissionResponse responseFromSpaceXMissionsApi = JsonConvert.DeserializeObject<SpaceXMissionResponse>(json);

            if (responseFromSpaceXMissionsApi == null)
            {
                response.ErrorMessage = ErrorMessages.GenericMessage;
                return response;
            }

            SpaceXMissionResponseModel spaceXMissionResponse = ParseResponse(responseFromSpaceXMissionsApi);

            response.Success = true;
            response.Data = spaceXMissionResponse;
            return response;
        }

        public async Task<ApiResponse<List<SpaceXMissionResponseModel>>> GetLaunchesFromSpaceCMissionsApi(LaunchType launchType)
        {
            var response = new ApiResponse<List<SpaceXMissionResponseModel>>() { ErrorMessage = "", Success = false };
            string json;

            using (var client = new HttpClient())
            {
                string spaceXMissionsApiPath = _configuration["SpaceXMissionApiConfig:ApiPath"];
                var endpointSuffix = launchType == LaunchType.Upcoming ? "/upcoming" : "/past";
                var url = $"{spaceXMissionsApiPath}{endpointSuffix}";

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var endpoint = new Uri(url);
                var result = await client.GetAsync(endpoint);
                json = await result.Content.ReadAsStringAsync();
            }

            var responseFromSpaceXMissionsApi = JsonConvert.DeserializeObject<List<SpaceXMissionResponse>>(json);

            if (responseFromSpaceXMissionsApi == null)
            {
                response.ErrorMessage = ErrorMessages.GenericMessage;
                return response;
            }

            List<SpaceXMissionResponseModel> spaceXMissionResponses = responseFromSpaceXMissionsApi
                .Select(mission => new SpaceXMissionResponseModel
                {
                    Id = mission.Id,
                    Name = mission.Name,
                    FlightNumber = mission.FlightNumber,
                    DateUtc = mission.DateUtc ?? DateTime.MinValue,
                    DateLocal = mission.DateLocal ?? DateTime.MinValue,
                    Success = mission.Success ?? false,
                    Rocket = mission.Rocket,
                    Crew = mission.Crew?.Select(c => new CrewResponseModel
                    {
                        Crew = c.Crew,
                        Role = c.Role
                    }).ToList(),
                    Webcast = mission.Links?.Webcast,
                    Wikipedia = mission.Links?.Wikipedia,
                    SmallImageUrl = mission.Links?.Patch?.Small,
                    LargeImageUrl = mission.Links?.Patch?.Large
                })
                .ToList();

            response.Data = spaceXMissionResponses;
            response.Success = true;
            return response;
        }

        private SpaceXMissionResponseModel ParseResponse(SpaceXMissionResponse apiResponse)
        {
            var missionResponse = new SpaceXMissionResponseModel
            {
                Id = apiResponse?.Id,
                Name = apiResponse?.Name,
                FlightNumber = apiResponse.FlightNumber,
                DateUtc = (DateTime)apiResponse.DateUtc,
                DateLocal = (DateTime)apiResponse.DateLocal,
                Success = (bool)apiResponse?.Success,
                Rocket = apiResponse?.Rocket,
                Webcast = apiResponse?.Links.Webcast,
                Wikipedia = apiResponse?.Links.Wikipedia,
                SmallImageUrl = apiResponse?.Links.Patch.Small,
                LargeImageUrl = apiResponse?.Links.Patch.Large,
                Crew = apiResponse?.Crew?.Select(c => new CrewResponseModel
                {
                    Crew = c.Crew,
                    Role = c.Role
                }).ToList()
            };

            return missionResponse;
        }
    }
}
