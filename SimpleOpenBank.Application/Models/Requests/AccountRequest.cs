using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Models.Requests
{
    public class AccountRequest
    {
        public int Amount { get; set; }
        public string Currency { get; set; }
    }
}
