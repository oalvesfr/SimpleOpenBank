using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Models.Responses
{
    public class CreateUserResponse
    {
        public int Id { get; set; }

        public string Created_At { get; set; }

        public string Email { get; set; }

        public string Full_Name { get; set; }

        public string Password_Changed_At { get; set; }

        public string Username { get; set; }

    }
}
