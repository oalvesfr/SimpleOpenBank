using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using SimpleOpenBank.Application.Business;
using SimpleOpenBank.Application.Contracts.Business;
using SimpleOpenBank.Application.Contracts.Persistence;
using SimpleOpenBank.Application.Models.Requests;
using SimpleOpenBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;

namespace SimpleOpenBank.Application.UnitTests
{

    [TestFixture]
    public class TransferTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private ITransferBusiness _transferBusiness;
        private TransferRequest _transferRequest;
        private AccountBD account;
        private int userId = 1;


        [SetUp]
        public void SetUp()
        {
            _unitOfWork = new Mock<IUnitOfWork>();

            _transferRequest = new ()
            {
                From_Account_Id = 1,
                To_Account_Id = 2,
                Amount = 30,
            };

            account = new()
            {
                Id = 1,
                UserId = userId,
                Balance = 100,
                Currency = "EUR",
                Created_At = DateTime.Now.ToString(),
            };
        }
        [Test]
        public async Task CreateTransfer_ValidRequest_ReturnStringAsync()
        {
            //Arrange
            _unitOfWork.Setup(t => t.TransferRepository.TransferTransation(
                        It.IsAny<TransferRequest>())).ReturnsAsync(true);
            _unitOfWork.Setup(r => r.AccountsRepository.Get(1)).ReturnsAsync(account);
            _unitOfWork.Setup(r => r.AccountsRepository.Get(2)).ReturnsAsync(account);

            _transferBusiness = new TransferBusiness(_unitOfWork.Object);

            //Act 
            var result = await _transferBusiness.CreateTransferBusiness(_transferRequest,1);

            //Assert
            Assert.That(result, Is.EqualTo("Transfer completed successfully"));
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        public void CreateTransfer_InvalidAccountFromOrTo_ReturnArgumentException(int user1, int user2)
        {
            //Arrange
            _unitOfWork.Setup(t => t.TransferRepository.TransferTransation(
                        It.IsAny<TransferRequest>())).ReturnsAsync(true);
            _unitOfWork.Setup(r => r.AccountsRepository.Get(user1)).ReturnsAsync(account);
            _unitOfWork.Setup(r => r.AccountsRepository.Get(user2)).ReturnsAsync(account);

            _transferBusiness = new TransferBusiness(_unitOfWork.Object);

            //Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => 
                        _transferBusiness.CreateTransferBusiness(_transferRequest, userId));
            Assert.That(ex.Message, Does.EndWith("account does not exist"));

        }

        [Test]
        public void CreateTransfer_InvalidBalance_ReturnArgumentException()
        {
            //Arrange
            _unitOfWork.Setup(t => t.TransferRepository.TransferTransation(
                        It.IsAny<TransferRequest>())).ReturnsAsync(true);
            _unitOfWork.Setup(r => r.AccountsRepository.Get(1)).ReturnsAsync(account);
            _unitOfWork.Setup(r => r.AccountsRepository.Get(2)).ReturnsAsync(account);

            _transferBusiness = new TransferBusiness(_unitOfWork.Object);
            _transferRequest.Amount = 300;
            //Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => 
                        _transferBusiness.CreateTransferBusiness(_transferRequest, userId));
            Assert.That(ex.Message, Is.EqualTo("Account balance is not enough"));

        }

        [Test]
        public void CreateTransfer_InvalidCurrency_ReturnArgumentException()
        {
            //Arrange
            var account2 = new AccountBD()
            {
                Id = 2,
                UserId = 2,
                Balance = 100,
                Currency = "DOL",

            };

            _unitOfWork.Setup(t => t.TransferRepository.TransferTransation(
                        It.IsAny<TransferRequest>())).ReturnsAsync(true);
            _unitOfWork.Setup(r => r.AccountsRepository.Get(1)).ReturnsAsync(account);
            _unitOfWork.Setup(r => r.AccountsRepository.Get(2)).ReturnsAsync(account2);

            _transferBusiness = new TransferBusiness(_unitOfWork.Object);

            //Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => 
                        _transferBusiness.CreateTransferBusiness(_transferRequest, userId));
            Assert.That(ex.Message, Is.EqualTo("The currency doesn't match"));

        }

        [Test]
        public void CreateTransfer_InvalidUser_ReturnAuthenticationException()
        {
            //Arrange
            _unitOfWork.Setup(t => t.TransferRepository.TransferTransation(
                        It.IsAny<TransferRequest>())).ReturnsAsync(true);
            _unitOfWork.Setup(r => r.AccountsRepository.Get(1)).ReturnsAsync(account);
            _unitOfWork.Setup(r => r.AccountsRepository.Get(2)).ReturnsAsync(account);

            _transferBusiness = new TransferBusiness(_unitOfWork.Object);

            //Assert
            var ex = Assert.ThrowsAsync<AuthenticationException>(() => 
                        _transferBusiness.CreateTransferBusiness(_transferRequest, 3));
            Assert.That(ex.Message, Is.EqualTo("User not account owner"));

        }
    }
}
