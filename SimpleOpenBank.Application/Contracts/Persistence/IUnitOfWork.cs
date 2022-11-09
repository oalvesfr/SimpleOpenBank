using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Contracts.Persistence
{
    public interface IUnitOfWork 
    {
        IAccountsRepository AccountsRepository { get; }
        ITransferRepository TransferRepository { get; } 
        IMovimRepository MovimRepository { get; }
        IUserRepository UserRepository { get; }
        ITokenRepository TokenRepository { get; }
        IDocumentRepository DocumentRepository { get; }

    }
}
