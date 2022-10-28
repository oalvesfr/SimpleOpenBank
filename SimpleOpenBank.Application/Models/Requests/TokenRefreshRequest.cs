using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Models.Requests
{
    public class TokenRefreshRequest
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string RefresToken { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }
    }
}
