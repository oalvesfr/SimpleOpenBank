using SimpleOpenBank.Application.Contracts.Persistence;
using SimpleOpenBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Persistence.Repository
{
    public class DocumentRepository : GenericRepository<DocumentBD>, IDocumentRepository
    {
        private readonly SimpleOpenBankDbContext _dbContext;

        public DocumentRepository(SimpleOpenBankDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public new async Task<DocumentBD> Add(DocumentBD document)
        {
            _dbContext.Documents.Add(document);
            await _dbContext.SaveChangesAsync();
            return document;
        }
    }
}
