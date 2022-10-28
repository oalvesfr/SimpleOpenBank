using SimpleOpenBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Contracts.Persistence
{
    public interface IAccountsRepository : IGenericRepository<AccountBD>
    {
        Task<AccountBD> AddAccount(AccountBD account);
        Task<AccountBD> GetAccountById(int id);
        Task<List<AccountBD>> GetAllAccount(int IdUser);
    }
}
