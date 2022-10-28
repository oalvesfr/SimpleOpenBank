using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Models.Requests
{
    public class CreateUserRequest
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Full_Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
