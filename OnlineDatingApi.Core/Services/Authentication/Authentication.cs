using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineDatingApi.Core.DTOs;
using OnlineDatingApi.Core.Models;
using OnlineDatingApi.Core.Repositories;
using OnlineDatingApi.Data.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineDatingApi.Core.Services.Authentication
{
    public class Authentication : IAuthentication
    {
        private readonly IUnitofWork _unitofWork;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApiUser _user;


        public Authentication(IUnitofWork unitofWork, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _unitofWork = unitofWork;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<bool> AuthenticateAsync(AuthDTO.LoginUserDTO user)
        {
            _user = await _userManager.FindByEmailAsync(user.Email);
            return (user != null && await _userManager.CheckPasswordAsync(_user, user.Password));
        }

        public async Task<RefreshTokenResponse> CreateToken(ApiUser _user)
        {
            var SigningCredentials = GetSigningCredentials();
            var claims = await GetClaims(_user);
            var tokenOptions = GenerateTokenOptions(SigningCredentials, claims);
            //return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            var refreshToken = new RefreshToken()
            {
                JwtId = "The FREAKKK",


                IsUsed = false,
                IsRevoked = false,
                ApiUserId = _user.Id,
                CreatedDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddMonths(6),
                Token = randomString(35) + Guid.NewGuid()
            };
            await _unitofWork.RefreshTokens.Create(refreshToken);
            await _unitofWork.Save();

            return new RefreshTokenResponse()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };


        }

        string randomString(int length)
        {
            var random = new Random();
            var chars = "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz1234567890";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }



        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("jwt");
            var expiration = DateTime.Now.AddSeconds(9000);
            Console.WriteLine($"Expiration time is {expiration}");
            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("ValidIsuer").Value,
                claims: claims,
                signingCredentials: signingCredentials,
                expires:expiration);

            return token;
        }
        

        private async Task<List<Claim>> GetClaims(ApiUser _user)
        {
            var claim = new List<Claim>(new List<Claim>
            {
                new Claim(ClaimTypes.Actor, _user.Name),
                new Claim(ClaimTypes.Name,_user.Id),
                new Claim(ClaimTypes.Email,_user.Email),
                new Claim(JwtRegisteredClaimNames.Sub,_user.Email),
                //Guid.NewGuid().ToString()
                new Claim(JwtRegisteredClaimNames.Jti,"The FREAKKK"
                ),

                //new Claim(ClaimTypes.Name,_user.UserName),
            });
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                
                claim.Add(new Claim(ClaimTypes.Role, role));
            }
            return claim;
        }

        private SigningCredentials GetSigningCredentials()
        {
            String key = Environment.GetEnvironmentVariable("KEY");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            return new SigningCredentials (secret, SecurityAlgorithms.HmacSha256);


        }

        public async Task<RefreshTokenResponse> GenerateRefreshToken(ApiUser user)
        {
            throw new NotImplementedException();
        }
    }
}
