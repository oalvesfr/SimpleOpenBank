using Microsoft.EntityFrameworkCore;
using SimpleOpenBank.Application.Contracts.Persistence;
using SimpleOpenBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Persistence.Repository
{
    public class AccountsRepository : GenericRepository<AccountBD>, IAccountsRepository
    {
        private readonly SimpleOpenBankDbContext _dbContext;

        public AccountsRepository(SimpleOpenBankDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AccountBD> AddAccount(AccountBD account)
        {

            _dbContext.Accounts.Add(account);
            await _dbContext.SaveChangesAsync();
            return account;
        }

        public async Task<AccountBD> GetAccountById(int id)
        {
            return await _dbContext.Accounts.FindAsync(id);
        }

        public async Task<List<AccountBD>> GetAllAccount(int idUser)
        {
            return await _dbContext.Accounts.Where(x => x.IdUser == idUser).ToListAsync();
        }
    }
}
