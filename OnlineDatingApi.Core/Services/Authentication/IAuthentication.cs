using OnlineDatingApi.Core.Models;
using OnlineDatingApi.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OnlineDatingApi.Core.DTOs.AuthDTO;

namespace OnlineDatingApi.Core.Services.Authentication
{
    public interface IAuthentication
    {
        Task<bool> AuthenticateAsync(LoginUserDTO user);
        Task<RefreshTokenResponse> CreateToken(ApiUser _user);
    }
}
