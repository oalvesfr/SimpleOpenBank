using SimpleOpenBank.Application.Models;
using SimpleOpenBank.Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Contracts.Infratructure
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(Notificacao notificacao);
    }
}
