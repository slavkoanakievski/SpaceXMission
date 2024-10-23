using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SpaceXMission.Entities;
using SpaceXMission_Repository.Interfaces;
using SpaceXMission_Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace SpaceXMission.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> Register(RegisterDto registerDto)
        {
            ApplicationUser userByEmail = await _userRepository.GetUserByEmailAsync(registerDto.Email);

            if (userByEmail is not null)
            {
                throw new ArgumentException($"User with email {registerDto.Email} already exists.");
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
                throw new ArgumentException($"Unable to register user {registerDto.Email} errors: {GetErrorsText(result.Errors)}");
            }

            return user.Email;
        }


        public async Task<string> Login(LoginDto loginDto)
        {
            ApplicationUser user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);
            bool isPasswordValid = await _userRepository.CheckPasswordAsync(user, loginDto.Password);

            if (user is null || !isPasswordValid)
            {
                throw new ArgumentException($"Unable to authenticate user {loginDto.Username}");
            }

            var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var token = GetToken(authClaims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
        {

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            int expiryInHours = int.TryParse(_configuration["JWT:ExpiryInHours"], out int hours) ? hours : 3;


            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(expiryInHours),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return token;
        }

        private string GetErrorsText(IEnumerable<IdentityError> errors)
        {
            return string.Join(", ", errors.Select(error => error.Description).ToArray());
        }
    }
}
