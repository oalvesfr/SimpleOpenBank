using SimpleOpenBank.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Domain
{
    public class AccountBD : BaseDomainEntity
    {
        public int UserId { get; set; }
        public float Balance { get; set; }
        public string Currency { get; set; }
        public string Created_At { get; set; }
    }
}
