using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SimpleOpenBank.Application.Business;
using SimpleOpenBank.Application.Contracts.Business;
using System.Reflection;


namespace SimpleOpenBank.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {


            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());


            services.AddTransient<IAccountBusiness, AccountBusiness>();
            services.AddTransient<ITransferBusiness, TransferBusiness>();
            services.AddTransient<IUserBusiness, UserBusiness>();
            services.AddTransient<IDocumentBusiness, DocumentBusiness>();

            return services;
        }
    }
}
