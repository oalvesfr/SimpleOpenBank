using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Models
{
    public class User
    {
        public string Full_Name { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Created_At { get; set; }

        public string Password_Changed_At { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
