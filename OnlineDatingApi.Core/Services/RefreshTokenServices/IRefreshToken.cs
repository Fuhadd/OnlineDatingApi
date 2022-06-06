using OnlineDatingApi.Core.DTOs;
using OnlineDatingApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDatingApi.Core.Services.RefreshTokenServices
{
    public interface IRefreshToken
    {
        public Task<RefreshTokenResponse> VerifyAndGenerateRefreshTokenAsync(TokenRequestDTO tokenRequest);


    }
}
