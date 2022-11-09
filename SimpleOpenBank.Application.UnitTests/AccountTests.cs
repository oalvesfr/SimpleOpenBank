using Microsoft.AspNetCore.SignalR;
using Moq;
using NUnit.Framework;
using SimpleOpenBank.Application.Business;
using SimpleOpenBank.Application.Contracts.Business;
using SimpleOpenBank.Application.Contracts.Persistence;
using SimpleOpenBank.Application.Models;
using SimpleOpenBank.Application.Models.Requests;
using SimpleOpenBank.Application.Models.Responses;
using SimpleOpenBank.Domain;
using Assert = NUnit.Framework.Assert;

namespace SimpleOpenBank.Application.UnitTests
{
    [TestFixture]
    public class AccountTests
    {
        private IAccountBusiness _accountBusiness;
        private Mock<IUnitOfWork> _unitOfWork;
        private AccountRequest accountRequest;
        private AccountBD account;
        private int userId = 1;
        private int accountId = 1;
        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            accountRequest = new ()
            {
                Amount = 100,
                Currency = "EUR",
            };
            account = new ()
            {
                Id = 1,
                UserId = userId,
                Balance = accountRequest.Amount,
                Currency = accountRequest.Currency,
                Created_At = DateTime.Now.ToString(),
            };
        }



        [Test]
        public async Task CreateAccount_ValidRequest_ReturnAccountResponse()
        {
            //Arrange

            _unitOfWork.Setup(a => a.AccountsRepository.Add(It.IsAny<AccountBD>())).ReturnsAsync(account);

            _accountBusiness = new AccountBusiness(_unitOfWork.Object);

            //Act 
            var result = await _accountBusiness.Create(accountRequest, userId);

            //Assert
            Assert.That(result, Is.TypeOf<AccountResponse>());
        }

        [Test]
        public async Task GetAccountById_ValidRequest_ReturnAccountMovims()
        {
            //Arrange

            _unitOfWork.Setup(r => r.AccountsRepository.Get(accountId)).ReturnsAsync(account);
            _unitOfWork.Setup(r => r.MovimRepository.GetAll(1)).ReturnsAsync(new List<MovimBD>());

            _accountBusiness =  new AccountBusiness(_unitOfWork.Object);

            //Act 
            var result = await _accountBusiness.Get(userId, accountId);

            //Assert
            Assert.That(result, Is.TypeOf<AccountMovims>());
        }

        [Test]
        public async Task GetAllAccounts_ValidRequest_ReturnListAccountResponse()
        {
            //Arrange

            _unitOfWork.Setup(r => r.AccountsRepository.Get(accountId)).ReturnsAsync(account);
            _unitOfWork.Setup(r => r.AccountsRepository.GetAll(userId)).ReturnsAsync(new List<AccountBD>());


            _accountBusiness = new AccountBusiness(_unitOfWork.Object);

            //Act 
            var result = await _accountBusiness.Get(userId);

            //Assert
            Assert.That(result, Is.TypeOf<List<AccountResponse>>());
        }

        [Test]
        public void GetAllAccounts_InvalidUser_ReturnException()
        {
            //Arrange
            int userId2 = 2;
            _unitOfWork.Setup(r => r.AccountsRepository.GetAll(userId2)).ReturnsAsync(new List<AccountBD>());


            _accountBusiness = new AccountBusiness(_unitOfWork.Object);


            //Assert
            Assert.ThrowsAsync<Exception>(() => _accountBusiness.Get(userId));

        }
    }
}