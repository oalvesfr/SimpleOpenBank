using SimpleOpenBank.Application.Models.Requests;
using SimpleOpenBank.Application.Models.Responses;
using SimpleOpenBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Contracts.Business
{
    public interface IAccountBusiness
    {
        Task<AccountResponse> Create(AccountRequest accountRequest, int userId);
        Task<AccountMovims?> Get(int userId, int id);
        Task<List<AccountResponse>> Get(int userId);
    }
}
