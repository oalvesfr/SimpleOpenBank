using SimpleOpenBank.Application.Contracts.Business;
using SimpleOpenBank.Application.Contracts.Infratructure;
using SimpleOpenBank.Application.Contracts.Persistence;
using SimpleOpenBank.Application.Models;
using SimpleOpenBank.Application.Models.Requests;
using SimpleOpenBank.Domain;
using System.Net.Mail;
using System.Security.Authentication;

namespace SimpleOpenBank.Application.Business
{

    public class TransferBusiness : ITransferBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailTransferProducer _emailTransferProducer;
        public TransferBusiness(IUnitOfWork unitOfWork, IEmailTransferProducer emailTransferProducer)
        {
            _unitOfWork = unitOfWork;
            _emailTransferProducer = emailTransferProducer;
        }
        public async Task<string> CreateTransferBusiness(TransferRequest transferRequest, int userId)
        {
            var from = await _unitOfWork.AccountsRepository.Get(transferRequest.From_Account_Id);

            if (from is null) throw new ArgumentException("From account does not exist");
            if (userId != from.UserId) throw new AuthenticationException("User not account owner");
            if (from.Balance < transferRequest.Amount) throw new ArgumentException("Account balance is not enough");

            var to = await _unitOfWork.AccountsRepository.Get(transferRequest.To_Account_Id);

            if (to is null) throw new ArgumentException("To account does not exist");
            if (from.Currency != to.Currency) throw new ArgumentException("The currency doesn't match");

            var isSuccess = await _unitOfWork.TransferRepository.TransferTransation(transferRequest);

            if (!isSuccess) throw new ArgumentException("Unable to transfer");

            // enviar email 
            if(from.UserId == to.UserId)
            {
                var result = await _emailTransferProducer.SendNotificacaoTransfer(CreateNotificacao(from, transferRequest));
                if(result == false) throw new ArgumentException("Email don't send");
            }
            else
            {
                var resultFrom = await _emailTransferProducer.SendNotificacaoTransfer(CreateNotificacao(from, transferRequest));
                var resultTo = await _emailTransferProducer.SendNotificacaoTransfer(CreateNotificacao(to, transferRequest));
                if (resultFrom == false) throw new ArgumentException("Email don't send");
                if (resultTo == false) throw new ArgumentException("Email don't send");
            }

            return "Transfer completed successfully";
        }

        private static Notificacao CreateNotificacao(AccountBD account, TransferRequest transferRequest)
        {
            return new Notificacao()
            {
                Amount = transferRequest.Amount.ToString(),
                FromAccountId = transferRequest.From_Account_Id.ToString(),
                ToAccountId = transferRequest.To_Account_Id.ToString(),
                UserId = account.UserId,
            };
        }
    }
}
