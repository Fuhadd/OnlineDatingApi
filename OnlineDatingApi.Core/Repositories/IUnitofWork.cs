using OnlineDatingApi.Data;
using OnlineDatingApi.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDatingApi.Core.Repositories
{
    public interface IUnitofWork : IDisposable
    {
        IRepository<UserImages> UserImages { get; }
        IRepository<UserImagesUrl> UserImagesUrl { get; }

        IRepository<RefreshToken> RefreshTokens { get; }

        IRepository<UserInterests> UserInterests { get; }

        IRepository<ApiUser> ApiUsers { get; }

        Task Save();

    }
}
