using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDatingApi.Core.Services.UserClaims
{
    public class UserClaims : IUserClaims
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserClaims(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetMyName()
        {
            var name = string.Empty;
            if(_httpContextAccessor.HttpContext!= null)
            {
                name = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Actor).Value;

            }
            return name;
        }

        public string GetMyEmail()
        {
            var email = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Actor).Value;

            }
            return email;
        }

        public string GetMyId()
        {
            var Id = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                Id = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            }
            return Id;
        }

      
               
    }
}
