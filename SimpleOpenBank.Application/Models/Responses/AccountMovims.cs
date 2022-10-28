using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Models.Responses
{
    public class AccountMovims
    {
        public float Balance { get; set; }

        public string? Created_At { get; set; }

        public string? Currency { get; set; }

        public int Account_Id { get; set; }
        public List<MovimResponse>? Movims { get; set; }
    }
}
