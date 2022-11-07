using SimpleOpenBank.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Domain
{
    public class TokenRefreshBD : BaseDomainEntity
    {
        public int UserId { get; set; }
        public string RefresToken { get; set; }
        public string RefreshTokenExpiresAt { get; set; }
    }
}
