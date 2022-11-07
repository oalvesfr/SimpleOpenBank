using SimpleOpenBank.Application.Contracts.Business;
using SimpleOpenBank.Application.Contracts.Persistence;
using SimpleOpenBank.Application.Models.Requests;
using SimpleOpenBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SimpleOpenBank.Application.Business
{

    public class TransferBusiness : ITransferBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        public TransferBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> CreateTransferBusiness(TransferRequest transferRequest, int userId)
        {
            var from = await _unitOfWork.AccountsRepository.Get(transferRequest.From_Account_Id);
            if (from == null)
                throw new ArgumentException("From account does not exist");

            if (userId != from.UserId)
                throw new AuthenticationException("User not account owner");

            if (from.Balance < transferRequest.Amount)
                throw new ArgumentException("Account balance is not enough");

            var to = await _unitOfWork.AccountsRepository.Get(transferRequest.To_Account_Id);
            if (to == null)
                throw new ArgumentException("To account does not exist");

            if (from.Currency != to.Currency)
                throw new ArgumentException("The currency doesn't match");

            var isSuccess = await _unitOfWork.TransferRepository.TransferTransation(transferRequest);
            if (!isSuccess)
                throw new ArgumentException("Unable to transfer");

            return "Transfer completed successfully";
        }
    }
}
