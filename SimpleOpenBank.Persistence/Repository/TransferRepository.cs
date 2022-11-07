using Microsoft.EntityFrameworkCore.Storage;
using SimpleOpenBank.Application.Contracts.Persistence;
using SimpleOpenBank.Application.Models.Requests;
using SimpleOpenBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Persistence.Repository
{
    public class TransferRepository : ITransferRepository
    {
        private readonly SimpleOpenBankDbContext _dbContext;

        public TransferRepository(SimpleOpenBankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> TransferTransation(TransferRequest transfer)
        {
            using(IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var from =  _dbContext.Accounts.Find(transfer.From_Account_Id);
                    from.Balance -= transfer.Amount;
                    _dbContext.Accounts.Update(from);
                    _dbContext.SaveChanges();

                    var to = _dbContext.Accounts.Find(transfer.To_Account_Id);
                    to.Balance += transfer.Amount;
                    _dbContext.Accounts.Update(to);
                    _dbContext.SaveChanges();

                    var movimFrom = new MovimBD()
                    {
                        Amount = -transfer.Amount,
                        IdAcount = transfer.From_Account_Id,
                        Created_At = DateTime.Now.ToString(),
                    };
                    _dbContext.Add(movimFrom);
                    _dbContext.SaveChanges();

                    var movimTo = new MovimBD()
                    {
                        Amount = transfer.Amount,
                        IdAcount = transfer.To_Account_Id,
                        Created_At = DateTime.Now.ToString(),
                    };
                    _dbContext.Movims.Add(movimTo);
                    _dbContext.SaveChanges();


                    transaction.Commit();
                    return Task.FromResult(true);
                }
                catch (Exception)
                {

                    transaction.Rollback();
                    return Task.FromResult(false);
                }
            }
        }


    }
}
