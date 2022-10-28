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


            services.AddScoped<IAccountBusiness, AccountBusiness>();
            services.AddScoped<ITransferBusiness, TransferBusiness>();
            services.AddScoped<IUserBusiness, UserBusiness>();


            return services;
        }
    }
}
