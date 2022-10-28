using SimpleOpenBank.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Domain
{
    public class MovimBD : BaseDomainEntity
    {
        public int IdAcount { get; set; }
        public float Amount { get; set; }
        public string Created_At { get; set; }
    }
}
