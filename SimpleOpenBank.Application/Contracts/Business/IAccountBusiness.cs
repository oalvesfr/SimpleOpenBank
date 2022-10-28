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
        Task<AccountResponse> CreatedAccountBusiness(AccountRequest accountRequest, int idUser);
        Task<AccountMovims?> GetAccountByIdBusiness(int idUser, int id);
        Task<List<AccountResponse>> GetAllAccountsBusiness(int idUser);
    }
}
