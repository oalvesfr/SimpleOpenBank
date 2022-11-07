using SimpleOpenBank.Application.Contracts.Persistence;
using SimpleOpenBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Persistence.Repository
{
    public class UserRepository : GenericRepository<UserBD>, IUserRepository
    {
        private readonly SimpleOpenBankDbContext _dbContext;

        public UserRepository(SimpleOpenBankDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public bool UsernameExists(string username)
        {
            return _dbContext.Users.Any(u => u.Username == username);
        }
        public async Task<UserBD> AddUser(UserBD user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public UserBD SearchUser(string username)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Username == username);
        }
    }
}
