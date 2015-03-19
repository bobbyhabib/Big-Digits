using System.Data.Entity;
using Dijits.Authentication.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dijits.Authentication.AuthContext
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext() : base("AuthContext")
        {
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}