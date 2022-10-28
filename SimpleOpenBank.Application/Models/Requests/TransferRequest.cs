using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Models.Requests
{
    public class TransferRequest
    {
        public int From_Account_Id { get; set; }
        public int To_Account_Id { get; set; }
        public float Amount { get; set; }
    }
}
