using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SpaceXMission.Dtos;
using SpaceXMission_Domain.Dtos;
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
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        public UserController(IAuthenticationService authenticationService,
                              ITokenService tokenService,
                              IUserService userService)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ApiResponse<AuthenticatedResponse>> Login([FromBody] LoginDto loginDto)
        {

            try
            {
                ApiResponse<AuthenticatedResponse> response = await _authenticationService.Login(loginDto);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new ApiResponse<AuthenticatedResponse>() { Success = false, ErrorMessage = ErrorMessages.GenericErrorControllerMessage };
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

        [HttpPost("refresh")]
        public async Task<ApiResponse<AuthenticatedResponse>> Refresh(TokenModel tokenModel)
        {

            try
            {
                ApiResponse<AuthenticatedResponse> response = await _tokenService.Refresh(tokenModel);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new ApiResponse<AuthenticatedResponse>() { Success = false, ErrorMessage = ErrorMessages.GenericErrorControllerMessage };
            }
        }
    }
}
