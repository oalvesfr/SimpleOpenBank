using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Contracts.Auth
{
    public interface IAuthToken
    {
        Task<string> CreateRefreshToken(DateTime expire);
        Task<string> CreateToken(string uid, DateTime expire);
    }
}
