using Microsoft.EntityFrameworkCore;
using SimpleOpenBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Persistence
{
    public class SimpleOpenBankDbContext : DbContext
    {
        public SimpleOpenBankDbContext(DbContextOptions<SimpleOpenBankDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SimpleOpenBankDbContext).Assembly);
        }

        public DbSet<UserBD> Users { get; set; }
        public DbSet<AccountBD> Accounts { get; set; }
        public DbSet<MovimBD> Movims { get; set; }
        public DbSet<TokenRefreshBD> TokenRefreshs { get; set; }
    }
}
