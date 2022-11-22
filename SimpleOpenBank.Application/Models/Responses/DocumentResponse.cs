using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Models.Responses
{
    public class DocumentResponse
    {
        public int Id { get; set; }
        public int Account_Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }
}
