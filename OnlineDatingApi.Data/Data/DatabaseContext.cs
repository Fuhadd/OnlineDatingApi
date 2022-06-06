using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDatingApi.Data.Data
{
    public class DatabaseContext : IdentityDbContext<ApiUser>
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }


        public DbSet<UserImages> UserImages { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserInterests> UserInterests { get; set; }
    }
}
