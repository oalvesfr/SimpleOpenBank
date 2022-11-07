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
    public class MovimRepository : GenericRepository<MovimBD>, IMovimRepository
    {
        private readonly SimpleOpenBankDbContext _dbContext;

        public MovimRepository(SimpleOpenBankDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<MovimBD>> GetAll(int idAcount)
        {
            return await _dbContext.Movims.Where(x => x.IdAcount == idAcount).ToListAsync();
        }
    }
}

