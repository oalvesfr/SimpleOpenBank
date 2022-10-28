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

        public async Task<bool> TransferTransation(TransferRequest transfer)
        {
            using(IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var from = await _dbContext.Accounts.FindAsync(transfer.From_Account_Id);
                    from.Balance -= transfer.Amount;
                    _dbContext.Accounts.Update(from);
                    await _dbContext.SaveChangesAsync();

                    var to = await _dbContext.Accounts.FindAsync(transfer.To_Account_Id);
                    to.Balance += transfer.Amount;
                    _dbContext.Accounts.Update(to);
                    await _dbContext.SaveChangesAsync();

                    var movimFrom = new MovimBD()
                    {
                        Amount = -transfer.Amount,
                        IdAcount = transfer.From_Account_Id,
                        Created_At = DateTime.Now.ToString(),
                    };
                    await _dbContext.AddAsync(movimFrom);
                    await _dbContext.SaveChangesAsync();

                    var movimTo = new MovimBD()
                    {
                        Amount = transfer.Amount,
                        IdAcount = transfer.To_Account_Id,
                        Created_At = DateTime.Now.ToString(),
                    };
                    await _dbContext.Movims.AddAsync(movimTo);
                    await _dbContext.SaveChangesAsync();


                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {

                    transaction.Rollback();
                    return false;
                }
            }
        }


    }
}
