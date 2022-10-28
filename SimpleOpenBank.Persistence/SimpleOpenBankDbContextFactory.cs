using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Persistence
{
    public class SimpleOpenBankDbContextFactory
    {
        public SimpleOpenBankDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<SimpleOpenBankDbContext>();
            var connectionString = configuration.GetConnectionString("SOBankConnectionString");

            builder.UseNpgsql(connectionString);

            return new SimpleOpenBankDbContext(builder.Options);
        }
    }
}
