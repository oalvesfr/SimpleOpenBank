using SimpleOpenBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Contracts.Persistence
{
    public interface IMovimRepository : IGenericRepository<MovimBD>
    {
        Task<List<MovimBD>> GetAll(int idAcount);
    }
}
