using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDatingApi.Core.Services.UserClaims
{
    public interface IUserClaims
    {
        string GetMyName();

        string GetMyEmail();

        string GetMyId();
    }
}
