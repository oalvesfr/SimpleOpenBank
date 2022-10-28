using SimpleOpenBank.Application.Models.Requests;
using SimpleOpenBank.Application.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Contracts.Business
{
    public interface IUserBusiness
    {
        Task<CreateUserResponse> CreatedUserBusiness(CreateUserRequest userRequest);
        Task<LoginUserResponse> LoginBusiness(LoginUserRequest loginUser);
        Task<LoginUserResponse> RefreshTokenBusiness(string refresh_Token);
    }
}
