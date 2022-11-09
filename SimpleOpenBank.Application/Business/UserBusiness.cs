using AutoMapper;
using MediatR;
using SimpleOpenBank.Application.Contracts.Auth;
using SimpleOpenBank.Application.Contracts.Business;
using SimpleOpenBank.Application.Contracts.Persistence;
using SimpleOpenBank.Application.Models.Requests;
using SimpleOpenBank.Application.Models.Responses;
using SimpleOpenBank.Domain;
using System.Security.Cryptography;
using System.Text;

namespace SimpleOpenBank.Application.Business
{

    public class UserBusiness : IUserBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthToken _authToken;
        public UserBusiness(IMapper mapper,
                            IUnitOfWork unitOfWork,
                            IAuthToken authToken)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _authToken = authToken;
        }

        public async Task<CreateUserResponse> CreatedUser(CreateUserRequest userRequest)
        {
            var userTrue =  _unitOfWork.UserRepository.UsernameExists(userRequest.Username);
            if(userTrue)
                throw new Exception($"Username '{userRequest.Username}' already exists.");



            CreatePasswordHash(userRequest.Password, out var passwordHash, out var passwordSalt);
            var user = new UserBD()
            {
                Full_Name = userRequest.Full_Name,
                Username = userRequest.Username,
                Email = userRequest.Email,
                Password = Convert.ToBase64String(passwordHash),
                Created_At = DateTime.Now.ToString(),
                Password_Changed_At = DateTime.Now.ToString(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt

            };

            user = await _unitOfWork.UserRepository.AddUser(user);
            var createUserResponse = _mapper.Map<CreateUserResponse>(user);

            return createUserResponse;
        }
        public Task<LoginUserResponse> LoginBusiness(LoginUserRequest loginUser)
        {
            var user =  _unitOfWork.UserRepository.SearchUser(loginUser.Username);
            if (user is null)
                throw new UnauthorizedAccessException("Username or Password does not correct.");


            if (!VerifyPasswordHash(loginUser.Password, user.PasswordHash, user.PasswordSalt))
                throw new UnauthorizedAccessException("Username or Password does not correct.");

            var expire = DateTime.Now.AddMinutes(60);
            var expireRefresh = DateTime.Now.AddHours(48);

            var loginUserResponse = new LoginUserResponse()
            {
                User = _mapper.Map<CreateUserResponse>(user),
                Access_Token =  _authToken.CreateToken(user.Id.ToString(), expire),
                Access_Token_Expires_At = expire.ToString(),
                Refresh_Token = _authToken.CreateRefreshToken(expireRefresh),
                Refresh_Token_Expires_At = expireRefresh.ToString(),
            };
     

            var token = new TokenRefreshBD()
            {
                UserId = user.Id,
                RefresToken = loginUserResponse.Refresh_Token,
                RefreshTokenExpiresAt = expireRefresh.ToString(),
            };
            _unitOfWork.TokenRepository.SaveTokenRefresh(token);
            return Task.FromResult(loginUserResponse);
            
            
        }

        public Task<LoginUserResponse> RefreshToken(string refresh_Token)
        {
            var token = _unitOfWork.TokenRepository.GetTokenRefresh(refresh_Token);
            if(token is null)
                throw new ArgumentNullException(nameof(refresh_Token), "Token is null");

            var tokenDateExpires = Convert.ToDateTime(token.RefreshTokenExpiresAt);
            if (tokenDateExpires < DateTime.Now)
                throw new Exception("Token is expired");


            var user = _unitOfWork.UserRepository.Get(token.UserId);
            if(user is null) throw new Exception("User to token does not exist");

            var expire = DateTime.Now.AddMinutes(60);
            var expireRefresh = DateTime.Now.AddHours(48);

            var loginUserResponse = new LoginUserResponse()
            {
                User = _mapper.Map<CreateUserResponse>(user),
                Access_Token =  _authToken.CreateToken(user.Id.ToString(), expire),
                Access_Token_Expires_At = expire.ToString(),
                Refresh_Token = _authToken.CreateRefreshToken(expireRefresh),
                Refresh_Token_Expires_At = expireRefresh.ToString(),
            };


            token.RefresToken = loginUserResponse.Refresh_Token;
            token.RefreshTokenExpiresAt = expireRefresh.ToString();

            _unitOfWork.TokenRepository.SaveTokenRefresh(token);

            return Task.FromResult(loginUserResponse);


        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return !computedHash.Where((t, i) => t != passwordHash[i]).Any();

        }

    }
}
