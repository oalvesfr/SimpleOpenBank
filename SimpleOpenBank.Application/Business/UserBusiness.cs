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

        public async Task<CreateUserResponse> CreatedUserBusiness(CreateUserRequest userRequest)
        {
            var userTrue = await _unitOfWork.UserRepository.UsernameExists(userRequest.Username);
            if(userTrue)
                throw new Exception($"Username '{userRequest.Username}' already exists.");


            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(userRequest.Password, out passwordHash, out passwordSalt);
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
        public async Task<LoginUserResponse> LoginBusiness(LoginUserRequest loginUser)
        {
            var user = await _unitOfWork.UserRepository.SearchUser(loginUser.Username);
            if (user == null)
                throw new UnauthorizedAccessException("Username or Password does not correct.");


            if (!VerifyPasswordHash(loginUser.Password, user.PasswordHash, user.PasswordSalt))
                throw new UnauthorizedAccessException("Username or Password does not correct.");

            DateTime expire = DateTime.Now.AddMinutes(1);
            DateTime expireRefresh = DateTime.Now.AddMinutes(60);

            var loginUserResponse = new LoginUserResponse()
            {
                User = _mapper.Map<CreateUserResponse>(user),
                Access_Token = await _authToken.CreateToken(user.Id.ToString(), expire),
                Access_Token_Expires_At = expire.ToString(),
                Refresh_Token = await _authToken.CreateRefreshToken(expireRefresh),
                Refresh_Token_Expires_At = expireRefresh.ToString(),
            };
     

            var token = new TokenRefreshBD()
            {
                IdUser = user.Id,
                RefresToken = loginUserResponse.Refresh_Token,
                RefreshTokenExpiresAt = expireRefresh.ToString(),
            };
            await _unitOfWork.TokenRepository.SaveTokenRefresh(token);
            return loginUserResponse;
            
            
        }

        public async Task<LoginUserResponse> RefreshTokenBusiness(string refresh_Token)
        {
            var token = await _unitOfWork.TokenRepository.GetTokenRefresh(refresh_Token);
            if(token == null)
                throw new Exception("Token is null");

            var tokenDateExpires = Convert.ToDateTime(token.RefreshTokenExpiresAt);
            if (tokenDateExpires < DateTime.Now)
                throw new ArgumentNullException("Token is expired");


            var user = await _unitOfWork.UserRepository.Get(token.IdUser);
            if(user == null)
                throw new Exception("User to token does not exist");

            DateTime expire = DateTime.Now.AddMinutes(1);
            DateTime expireRefresh = DateTime.Now.AddMinutes(60);

            var loginUserResponse = new LoginUserResponse()
            {
                User = _mapper.Map<CreateUserResponse>(user),
                Access_Token = await _authToken.CreateToken(user.Id.ToString(), expire),
                Access_Token_Expires_At = expire.ToString(),
                Refresh_Token = await _authToken.CreateRefreshToken(expireRefresh),
                Refresh_Token_Expires_At = expireRefresh.ToString(),
            };


            token.RefresToken = loginUserResponse.Refresh_Token;
            token.RefreshTokenExpiresAt = expireRefresh.ToString();

            await _unitOfWork.TokenRepository.SaveTokenRefresh(token);

            return loginUserResponse;


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
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;
            }
        }

    }
}
