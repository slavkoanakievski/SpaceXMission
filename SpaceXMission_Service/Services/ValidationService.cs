using Microsoft.Extensions.Configuration;
using SpaceXMission.Dtos;
using SpaceXMission.Entities;
using SpaceXMission_Repository.Interfaces;
using SpaceXMission_Service.Interfaces;
using SpaceXMission_Shared;
using SpaceXMission_Shared.Constants;
using SpaceXMission_Shared.Helpers.Models;
using System.Text.RegularExpressions;

namespace SpaceXMission_Service.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        public ValidationService(IConfiguration configuration,
                                IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<bool>> ValidateFieldsForRegisterAccount(RegisterDto registerDto)
        {
            ApiResponse<bool> response = new ApiResponse<bool>() { Success = false };
            registerDto.TrimStringProperties();

            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                // validation failed
                response.ErrorMessage = ErrorMessages.PasswordsDoNotMatchMessage;
                return response;
            }

            Regex passwordRegex = new Regex(_configuration["RegexValidation:PasswordRegex"]);
            Regex emailRegex = new Regex(_configuration["RegexValidation:EmailRegex"]);

            if (!emailRegex.IsMatch(registerDto.Email) || !passwordRegex.IsMatch(registerDto.Password))
            {
                // validation failed
                response.ErrorMessage = ErrorMessages.InvalidEmailOrPasswordFormat;
                return response;
            }

            ApplicationUser userByEmail = await _userRepository.GetUserByEmailAsync(registerDto.Email);

            if (userByEmail is not null)
            {
                // validation failed
                response.ErrorMessage = ErrorMessages.UserAlreadyExistsError;
                return response;
            }

            // validation successful
            response.Data = true;
            response.Success = true;
            return await Task.FromResult(response);
        }

        public async Task<ApiResponse<bool>> ValidateLoginFields(LoginDto loginDto)
        {
            ApiResponse<bool> response = new ApiResponse<bool>() { Success = false };

            loginDto.TrimStringProperties();

            Regex emailRegex = new Regex(_configuration["RegexValidation:EmailRegex"]);
            Regex passwordRegex = new Regex(_configuration["RegexValidation:PasswordRegex"]);

            if (!emailRegex.IsMatch(loginDto.Username) || !passwordRegex.IsMatch(loginDto.Password))
            {
                // validation failed
                response.ErrorMessage = ErrorMessages.InvalidEmailOrPassword;
                return response;
            }

            response.Success = true;
            response.Data = true;
            return await Task.FromResult(response);
        }
    }
}
