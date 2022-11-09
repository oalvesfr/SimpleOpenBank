using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Contracts.Business
{
    public interface IDocumentBusiness
    {
        Task<string> UploadFile(IFormFile file, int userId, int accountId);
    }
}
