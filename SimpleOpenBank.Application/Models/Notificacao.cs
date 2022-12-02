using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Models
{
    public class Notificacao
    {
        public int UserId { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
        public string Amount { get; set; }
        public string FullName { get; set; }
        public string EmailAdress { get; set; }
        public string Body { get; set; }
    }
}
