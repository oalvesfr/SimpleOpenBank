using SimpleOpenBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Contracts.Persistence
{
    public interface IUserRepository : IGenericRepository<UserBD>
    {
        Task<UserBD> AddUser(UserBD user);
        Task<UserBD> SearchUser(string username);
        Task<bool> UsernameExists(string username);
    }
}
