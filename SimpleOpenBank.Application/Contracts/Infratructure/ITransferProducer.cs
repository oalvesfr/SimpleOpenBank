using SimpleOpenBank.Application.Models;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Contracts.Infratructure
{
    public interface ITransferProducer
    {
       Task PublishEvent(Notificacao notificacao);
    }
}
