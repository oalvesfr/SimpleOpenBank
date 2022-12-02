using Microsoft.Extensions.Configuration;
using SimpleOpenBank.Application.Contracts.Infratructure;
using SimpleOpenBank.Application.Contracts.Persistence;
using SimpleOpenBank.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Infratructure.Mail
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<bool> SendEmail(Notificacao notificacao)
        {
            var email = CreateEmail(notificacao);
            try
            {
                using (SmtpClient smtp = new SmtpClient(
                    _configuration["EmailSettings:Host"],
                    int.Parse(_configuration["EmailSettings:Port"])))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_configuration["EmailSettings:Mail"], _configuration["EmailSettings:Password"]);
                    smtp.EnableSsl = true;
                    smtp.Send(email);
                }
                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }
        private MailMessage CreateEmail(Notificacao notificacao)
        {
            return new MailMessage(
                from: _configuration["EmailSettings:Mail"],
                to: notificacao.EmailAdress,
                subject: "You have a transfer.",
                body: notificacao.Body)
            { IsBodyHtml = true };

        }
    }
}
