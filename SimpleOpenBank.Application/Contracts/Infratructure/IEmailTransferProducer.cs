using SimpleOpenBank.Application.Models;
using System.Net.Mail;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Contracts.Infratructure
{
    public interface IEmailTransferProducer
    {
        Task<bool> SendNotificacaoTransfer(Notificacao notificacao);
        Task SendEmailTransfer(Notificacao notificacao);
    }
}