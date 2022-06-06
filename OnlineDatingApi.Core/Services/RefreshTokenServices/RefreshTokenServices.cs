using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OnlineDatingApi.Core.DTOs;
using OnlineDatingApi.Core.Models;
using OnlineDatingApi.Core.Repositories;
using OnlineDatingApi.Core.Services.Authentication;
using OnlineDatingApi.Data.Data;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
//using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OnlineDatingApi.Core.DTOs.AuthDTO;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace OnlineDatingApi.Core.Services.RefreshTokenServices
{
    public class RefreshTokenServices : IRefreshToken

    {
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IAuthentication _authentication;

        public RefreshTokenServices(TokenValidationParameters tokenValidationParams, 
                                    IUnitofWork unitofWork, 
                                    IMapper mapper,
                                    UserManager<ApiUser> userManager,
                                    IAuthentication authentication)
        {
            _tokenValidationParams = tokenValidationParams;
            _unitofWork = unitofWork;
            _mapper = mapper;
            _userManager = userManager;
            _authentication = authentication;
        }
        public async Task<RefreshTokenResponse> VerifyAndGenerateRefreshTokenAsync(TokenRequestDTO tokenRequest)
        {
            
            var jwtTokenHandler = new JwtSecurityTokenHandler(); 

            try
            {
                Console.WriteLine("going");
                //VALIDATION 1: Validate JWT Token Format
                _tokenValidationParams.ValidateLifetime = false;
                var tokenInVerification = jwtTokenHandler.
                    ValidateToken(tokenRequest.Token, _tokenValidationParams, out var validatedToken);
                _tokenValidationParams.ValidateLifetime = true;
                Console.WriteLine("going1");
                //Validation 2: Validate Encryption Algorithm

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256);

                    if (result == false)
                    {
                        return null;
                    }
                }

                //Validation 3: Validate Expiry Date
                
                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x =>
                x.Type == JwtRegisteredClaimNames.Exp).Value);
                



                var expiryDate = unixTimeStampToDateTime(utcExpiryDate);
                Console.WriteLine(expiryDate);
                Console.WriteLine(DateTime.Now);

                if (expiryDate > DateTime.Now)
                {
                    return new RefreshTokenResponse()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Current Token Has Not Yet Expired"
                        }
                    };


                }

                //Validation 4: Check if Token Exists in Database

                var storedToken = await _unitofWork.RefreshTokens.Get(expression: x => 
                        x.Token == tokenRequest.RefreshToken);
                
                

                if (storedToken == null)
                {
                    return new RefreshTokenResponse()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Current Token Does Not Exist"
                        }
                    };

                }

                //Validation 5: Check if Refresh Token Has Been Used
                Console.WriteLine($"Check if The token is used {storedToken.IsUsed}");
                if (storedToken.IsUsed) 
                {
                    return new RefreshTokenResponse()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Current Refresh Token Has Been Used"
                        }
                    };
                }

                //Validation 6: Check if Refresh Token Has Been Revoked
                Console.WriteLine($"Check if The token is used {storedToken.IsRevoked}");
                if (storedToken.IsRevoked)
                {
                    return new RefreshTokenResponse()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Current Refresh Token Has Been Revoked"
                        }
                    };
                }

                //Validation 7: Check if Refresh Token Has Been Revoked

                var jti = tokenInVerification.Claims.FirstOrDefault(x=>
                    x.Type == JwtRegisteredClaimNames.Jti).Value;
                Console.WriteLine(jti);

                if (storedToken.JwtId!= jti)
                {
                    return new RefreshTokenResponse()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Current Refresh Token Does Not Match"
                        }
                    };
                }

                //Update Current Token
                
                storedToken.IsUsed = true;
                Console.WriteLine("jfhufh");
                storedToken = _mapper.Map<RefreshToken>(storedToken);
                Console.WriteLine("kfjfifnjknf");

                _unitofWork.RefreshTokens.Update(storedToken);
                Console.WriteLine("jfhufh");
                await _unitofWork.Save();

                var apiUser = await _userManager.FindByIdAsync(storedToken.ApiUserId);


                //var user = _mapper.Map<ApiUser>(apiUser);
                return await _authentication.CreateToken(apiUser);


            }
            catch (Exception e){
                throw new Exception(e.Message);
            }
        }

        private DateTime unixTimeStampToDateTime(long utcExpiryDate)
        {
            var dateTimeVal = new DateTime(1970, 1,1, 0, 0, 0, DateTimeKind.Utc);

            dateTimeVal = dateTimeVal.AddSeconds(utcExpiryDate).ToLocalTime();
            return dateTimeVal;
        }
    }
}
