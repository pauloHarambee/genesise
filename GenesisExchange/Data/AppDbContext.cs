using GenesisExchange.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GenesisExchange.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options) { }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<Beneficiaries> Beneficiaries { get; set; }

    }
    
}
