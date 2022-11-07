using Microsoft.EntityFrameworkCore;
using SimpleOpenBank.Application.Contracts.Persistence;
using SimpleOpenBank.Application.Models;
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

        public new Task<AccountBD> Add(AccountBD account)
        {

            _dbContext.Accounts.Add(account);
            _dbContext.SaveChanges();
            return Task.FromResult(account);
        }

        public new async Task<AccountBD> Get(int id)
        {
            return await _dbContext.Accounts.FindAsync(id);
        }

        public async Task<List<AccountBD>> GetAll(int userId)
        {
            return await _dbContext.Accounts.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
