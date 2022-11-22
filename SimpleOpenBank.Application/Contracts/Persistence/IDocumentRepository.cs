using SimpleOpenBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Contracts.Persistence
{
    public interface IDocumentRepository : IGenericRepository<DocumentBD>
    {
        new Task<DocumentBD> Add(DocumentBD document);
        Task<List<DocumentBD>> GetAll(int userId);
    }
}
