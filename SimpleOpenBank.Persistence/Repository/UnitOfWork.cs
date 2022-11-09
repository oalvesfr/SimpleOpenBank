using SimpleOpenBank.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly SimpleOpenBankDbContext _context;
        private IAccountsRepository _accountsRepository;
        private ITransferRepository _transferRepository;
        private IMovimRepository _movimRepository;
        private IUserRepository _userRepository;
        private ITokenRepository _tokenRepository;
        private IDocumentRepository _documentRepository;

        public UnitOfWork(SimpleOpenBankDbContext context)
        {
            _context = context;

        }

        public IAccountsRepository AccountsRepository => _accountsRepository ??= new AccountsRepository(_context);
        public ITransferRepository TransferRepository => _transferRepository ??= new TransferRepository(_context);
        public IMovimRepository MovimRepository => _movimRepository ??= new MovimRepository(_context);
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

        public ITokenRepository TokenRepository => _tokenRepository ??= new TokenRepository(_context);

        public IDocumentRepository DocumentRepository => _documentRepository ??= new DocumentRepository(_context);


    }
}
