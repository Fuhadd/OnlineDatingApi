using OnlineDatingApi.Data;
using OnlineDatingApi.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDatingApi.Core.Repositories
{
    public class UnitofWork : IUnitofWork

        
    {
        private readonly DatabaseContext _context;
        private IRepository<UserImages>? _userImages;
        private IRepository<UserImagesUrl>? _userImagesUrl;
        private IRepository<UserInterests>? _userInterests;
        private IRepository<ApiUser>? _apiUser;
        private IRepository<RefreshToken>? _refreshToken;

        public UnitofWork(DatabaseContext context )
        {
            _context = context;
        }
        public IRepository<UserImages> UserImages => _userImages ??= new Repository<UserImages>(_context);

        public IRepository<UserImagesUrl> UserImagesUrl => _userImagesUrl ??= new Repository<UserImagesUrl>(_context);

        public IRepository<ApiUser> ApiUsers => _apiUser ??= new Repository<ApiUser>(_context);

        public IRepository<RefreshToken> RefreshTokens => _refreshToken ??= new Repository<RefreshToken>(_context);

        public IRepository<UserInterests> UserInterests => _userInterests??= new Repository<UserInterests>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize( this );
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
