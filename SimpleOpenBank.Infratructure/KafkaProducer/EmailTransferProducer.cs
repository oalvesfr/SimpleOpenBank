using Microsoft.Extensions.Configuration;
using SimpleOpenBank.Application.Contracts.Infratructure;
using SimpleOpenBank.Application.Contracts.Persistence;
using SimpleOpenBank.Application.Models;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleOpenBank.Infratructure.KafkaProducer
{
    public class EmailTransferProducer : IEmailTransferProducer
    {
         private readonly IUnitOfWork _unitOfWork;
         private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly ITransferProducer _transferProducer;

        public EmailTransferProducer(IUnitOfWork unitOfWork,
                                    IEmailSender emailSender,
                                    IConfiguration configuration,
                                    ITransferProducer transferProducer)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _configuration = configuration;
            _transferProducer = transferProducer;
        }



         public async Task<bool> SendNotificacaoTransfer(Notificacao notificacao)
        {
            var from = await _unitOfWork.UserRepository.Get(Convert.ToInt32(notificacao.UserId));
            notificacao.FullName = from.Full_Name;
            notificacao.EmailAdress = from.Email;
            notificacao.Body = CreateBody(notificacao);
            await _transferProducer.PublishEvent(notificacao);
            return true;
        }
        public async Task SendEmailTransfer(Notificacao notificacao)
        {
            var result = await _emailSender.SendEmail(notificacao);
        }
        private static string CreateBody(Notificacao notificacao)
        {
            return "Caro(a) Cliente," + notificacao.FullName + "\r\n" +
                "Foi realizada uma transferencia no valor de " + notificacao.Amount + ", da conta " + notificacao.FromAccountId +
                " para a conta " + notificacao.ToAccountId + ".\r\n" +
                "  \r\nCom os melhores cumprimentos\r\n";
        }
    }
}