using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Models.Responses
{
    public class LoginUserResponse
    {
        public string Access_Token { get; set; }

        public string Access_Token_Expires_At { get; set; }
        public string Refresh_Token { get; set; }

        public string Refresh_Token_Expires_At { get; set; }
        public CreateUserResponse User { get; set; }
    }
}
