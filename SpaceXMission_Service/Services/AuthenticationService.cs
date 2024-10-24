using Microsoft.AspNetCore.Identity;
using SpaceXMission.Dtos;
using SpaceXMission.Entities;
using SpaceXMission_Domain.Dtos;
using SpaceXMission_Repository.Interfaces;
using SpaceXMission_Service.Interfaces;
using SpaceXMission_Shared;
using SpaceXMission_Shared.Constants;
using SpaceXMission_Shared.Helpers.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace SpaceXMission.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly IUserRepository _userRepository;
        private readonly IValidationService _validationService;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IUserRepository userRepository,
                                     IValidationService validationService,
                                     ITokenService tokenService)
        {
            _userRepository = userRepository;
            _validationService = validationService;
            _tokenService = tokenService;
        }

        public async Task<ApiResponse<string>> Register(RegisterDto registerDto)
        {
            ApiResponse<string> response = new();

            ApiResponse<bool> fieldValidationResponse = await _validationService.ValidateFieldsForRegisterAccount(registerDto);

            if (!fieldValidationResponse.Success)
            {
                response.Success = fieldValidationResponse.Success;
                response.ErrorMessage = fieldValidationResponse.ErrorMessage;
                return response;
            }

            ApplicationUser user = new()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDto.Email
            };

            IdentityResult result = await _userRepository.CreateUserAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                response.Success = false;
                response.ErrorMessage = HelperMethods.GetErrorsText(result.Errors);
                return response;
            }

            string userId = await _userRepository.GetUserIdByUsernameAsync(registerDto.Email);
            response.Data = userId;
            response.Success = true;
            return response;
        }


        public async Task<ApiResponse<AuthenticatedResponse>> Login(LoginDto loginDto)
        {
            ApiResponse<AuthenticatedResponse> response = new ApiResponse<AuthenticatedResponse>() { ErrorMessage = "", Success = false };

            ApiResponse<bool> fieldValidationResponse = await _validationService.ValidateLoginFields(loginDto);
            if (!fieldValidationResponse.Success)
            {
                response.ErrorMessage = fieldValidationResponse.ErrorMessage;
                response.Success = fieldValidationResponse.Success;
                return response;
            }

            ApplicationUser user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);
            if (user == null)
            {
                response.ErrorMessage = ErrorMessages.UsernameDoesNotExist;
                return response;
            }
            bool isPasswordValid = await _userRepository.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordValid)
            {
                response.ErrorMessage = ErrorMessages.InvalidPassword;
                return response;
            }

            List<Claim> authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            JwtSecurityToken accessToken = _tokenService.GetToken(authClaims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            await _userRepository.UpdateRefreshTokenAsync(user, refreshToken);

            response.Data = new AuthenticatedResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken,
                UserName = user.Email,
            };
            response.Success = true;
            return response;
        }
    }
}
