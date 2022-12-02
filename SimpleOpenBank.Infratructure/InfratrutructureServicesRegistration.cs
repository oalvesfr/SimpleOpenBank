using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleOpenBank.Application.Contracts.Auth;
using SimpleOpenBank.Application.Contracts.Infratructure;
using SimpleOpenBank.Infratructure.Consumer;
using SimpleOpenBank.Infratructure.KafkaProducer;
using SimpleOpenBank.Infratructure.Mail;
using SimpleOpenBank.Infratructure.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Infratructure
{
    public static class InfratrutructureServicesRegistration
    {
        public static IServiceCollection ConfigureInfratrutructureServices(this IServiceCollection services)
        {
            services.AddTransient<ITransferProducer, TransferProducer>();
            services.AddTransient<IEmailTransferProducer, EmailTransferProducer>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddHostedService<TransferConsumer>();
            return services;
        }
    }
}
