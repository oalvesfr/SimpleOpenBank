using Moq;
using SimpleOpenBank.Application.Contracts.Persistence;
using SimpleOpenBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.UnitTests.Mocks
{
    public static class MockAccountsRepository
    {
        public static Mock<IAccountsRepository> GetAccountRepository(int id)
        {
            var accounts = new List<AccountBD>
            {
                new AccountBD
                {
                    Id = 1,
                    IdUser = 1,
                    Balance = 100,
                    Currency = "EUR"

                },
                new AccountBD
                {
                    Id = 2,
                    IdUser = 2,
                    Balance = 100,
                    Currency = "EUR"

                },
                new AccountBD
                {
                    Id = 3,
                    IdUser = 1,
                    Balance = 100,
                    Currency = "DOL"

                },
                new AccountBD
                {
                    Id = 3,
                    IdUser = 2,
                    Balance = 100,
                    Currency = "DOL"

                },
            };
            var account = new AccountBD
            {
                Id = 1,
                IdUser = 1,
                Balance = 100,
                Currency = "EUR"
            };
            var mockAccount = new Mock<IAccountsRepository>();

            mockAccount.Setup(r => r.GetAllAccount(id)).ReturnsAsync(accounts);
            mockAccount.Setup(r => r.GetAccountById(id)).ReturnsAsync(account);

            return mockAccount;
        }
    }
}
