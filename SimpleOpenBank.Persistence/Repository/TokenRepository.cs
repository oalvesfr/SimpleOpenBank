using SimpleOpenBank.Application.Contracts.Persistence;
using SimpleOpenBank.Application.Models.Responses;
using SimpleOpenBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Persistence.Repository
{
    public class TokenRepository : GenericRepository<TokenRefreshBD>, ITokenRepository
    {
        private readonly SimpleOpenBankDbContext _dbContext;

        public TokenRepository(SimpleOpenBankDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<TokenRefreshBD> GetTokenRefresh(string refresh_Token)
        {
            return _dbContext.TokenRefreshs.FirstOrDefault(t => t.RefresToken == refresh_Token);
        }

        public async Task SaveTokenRefresh(TokenRefreshBD tokenRefresh)
        {

            var token = _dbContext.TokenRefreshs.FirstOrDefault(x => x.IdUser == tokenRefresh.IdUser);
            if (token != null)
            {
                token.RefresToken = tokenRefresh.RefresToken;
                token.RefreshTokenExpiresAt = tokenRefresh.RefreshTokenExpiresAt.ToString();

                _dbContext.TokenRefreshs.Update(token);
            }
            else
            {
                await _dbContext.TokenRefreshs.AddAsync(tokenRefresh);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}

