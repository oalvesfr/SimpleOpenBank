using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Contracts.Auth
{
    public interface IAuthToken
    {
        string CreateRefreshToken(DateTime expire);
        string CreateToken(string uid, DateTime expire);
    }
}
