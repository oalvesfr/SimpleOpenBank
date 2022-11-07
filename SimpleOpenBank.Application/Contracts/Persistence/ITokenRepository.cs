using SimpleOpenBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Contracts.Persistence
{
    public interface ITokenRepository : IGenericRepository<TokenRefreshBD>
    {
        TokenRefreshBD GetTokenRefresh(string refresh_Token);
        Task SaveTokenRefresh(TokenRefreshBD tokenRefresh);
    }
}
