using Microsoft.AspNetCore.Http;
using SimpleOpenBank.Application.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Contracts.Business
{
    public interface IDocumentBusiness
    {
        Task<byte[]> Get(int id, int userId);
        Task<List<DocumentResponse>> Get(int userId);
        Task<string> DownloadFilel(IFormFile file, int userId, int accountId);
    }
}
