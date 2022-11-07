using AutoMapper;
using SimpleOpenBank.Application.Contracts.Business;
using SimpleOpenBank.Application.Contracts.Persistence;
using SimpleOpenBank.Application.Models.Requests;
using SimpleOpenBank.Application.Models.Responses;
using SimpleOpenBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Business
{

    public class AccountBusiness : IAccountBusiness
    {
       
        private readonly IUnitOfWork _unitOfWork;
        public AccountBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        //create account
        public async Task<AccountResponse> Create(AccountRequest accountRequest, int userId)
        {
            var account = new AccountBD()
            {
                UserId = userId,
                Balance = accountRequest.Amount,
                Currency = accountRequest.Currency,
                Created_At = DateTime.Now.ToString(),
            };

            var newAccount = await _unitOfWork.AccountsRepository.Add(account);
            if(newAccount is null)
                throw new Exception();

            return MapAccountResponse(newAccount);
        }

        //GetAccountById
        public async Task<AccountMovims> Get(int userId, int id)
        {
            var account = await _unitOfWork.AccountsRepository.Get(id);
            if (account is null)
                throw new ArgumentNullException("Account not exist");
            if(account.UserId != userId)
                throw new UnauthorizedAccessException("You do not have permission to access this account");
            var listMovim = await _unitOfWork.MovimRepository.GetAll(account.Id);
            var accountMovims = new AccountMovims()
            {
                Balance = account.Balance,
                Created_At = account.Created_At,
                Currency = account.Currency,
                Account_Id = account.Id,
                Movims = GetListMovimResponse(listMovim),

            };

            return accountMovims;
        }

        //GetAllAccounts
        public async Task<List<AccountResponse>> Get(int userId)
        {

            var listAccounts = await _unitOfWork.AccountsRepository.GetAll(userId);
            if(listAccounts is null)
                throw new Exception("User dont have accounts");

            var listAccountResponse = new List<AccountResponse>();
            foreach (var account in listAccounts)
            {
                listAccountResponse.Add(MapAccountResponse(account));
            }
            return listAccountResponse;

        }
        
        private static AccountResponse MapAccountResponse(AccountBD account)
        {
            return new AccountResponse()
            {
                Id = account.Id,
                Balance = account.Balance,
                Currency = account.Currency,
                Created_At = account.Created_At,
            };
        }

        private List<MovimResponse> GetListMovimResponse(List<MovimBD> listMovim)
        {
            var listMovimResponse = new List<MovimResponse>();
            foreach(var movim in listMovim)
            {
                listMovimResponse.Add(
                    new MovimResponse()
                    {
                        Amount = movim.Amount,
                        Created_At = movim.Created_At,
                    }
                    );
            }
            return listMovimResponse;
        }
    }
}
