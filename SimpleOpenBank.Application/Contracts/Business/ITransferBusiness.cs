using SimpleOpenBank.Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Contracts.Business
{
    public interface ITransferBusiness
    {
        Task<string> CreateTransferBusiness(TransferRequest transferRequest, int userId);
    }
}
