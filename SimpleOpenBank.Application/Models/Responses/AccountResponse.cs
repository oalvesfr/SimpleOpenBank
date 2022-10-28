using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Models.Responses
{
    public class AccountResponse
    {
        public float Balance { get; set; }
        public string Created_At { get; set; }
        public string Currency { get; set; }
        public int Id { get; set; }
    }
}
