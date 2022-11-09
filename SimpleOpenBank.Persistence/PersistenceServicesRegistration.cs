using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleOpenBank.Application.Contracts.Persistence;
using SimpleOpenBank.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Persistence
{
    public static class PersistenceServicesRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SimpleOpenBankDbContext>(options =>
               options.UseNpgsql(
                   configuration.GetConnectionString("SOBankConnectionString")));

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IAccountsRepository, AccountsRepository>();
            services.AddTransient<ITransferRepository, TransferRepository>();
            services.AddTransient<IMovimRepository, MovimRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IDocumentRepository, DocumentRepository>();

            return services;
        }
    }
}
