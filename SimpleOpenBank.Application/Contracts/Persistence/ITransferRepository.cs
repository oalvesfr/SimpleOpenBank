using SimpleOpenBank.Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Contracts.Persistence
{
    public interface ITransferRepository
    {
        Task<bool> TransferTransation(TransferRequest transfer);
    }
}
