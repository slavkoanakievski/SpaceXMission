using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SpaceXMission.Dtos;
using SpaceXMission_Service.Interfaces;
using SpaceXMission_Shared.Constants;
using SpaceXMission_Shared.Helpers.Models;

namespace SpaceXMission.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public UserController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ApiResponse<string>> Login([FromBody] LoginDto loginDto)
        {

            try
            {
                ApiResponse<string> response = await _authenticationService.Login(loginDto);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new ApiResponse<string>() { Success = false, ErrorMessage = ErrorMessages.GenericErrorControllerMessage };
            }

        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ApiResponse<string>> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                ApiResponse<string> response = await _authenticationService.Register(registerDto);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new ApiResponse<string>() { Success = false, ErrorMessage = ErrorMessages.GenericErrorControllerMessage };
            }
        }
    }
}
