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
        //new Task<AccountBD> Add(AccountBD account);
        //new Task<AccountBD> Get(int id);
        Task<List<AccountBD>> GetAll(int userId);
    }
}
