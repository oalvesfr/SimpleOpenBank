using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleOpenBank.Application.Contracts.Business;
using SimpleOpenBank.Application.Contracts.Persistence;
using SimpleOpenBank.Application.Models.Responses;
using SimpleOpenBank.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SimpleOpenBank.Application.Business
{
    public class DocumentBusiness : IDocumentBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private string[] permittedExtensions = { ".txt", ".pdf", ".png" };

        public DocumentBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> DownloadFilel(IFormFile file, int userId, int accountId)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                throw new ArgumentException("The extension is invalid");

            var account = await _unitOfWork.AccountsRepository.Get(accountId);
            if(account is null) throw new ArgumentException("The account does not exist");

            if (userId != account.UserId) throw new AuthenticationException("User not account owner");

            using (var memoryStream = new MemoryStream())
            {
                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    await file.CopyToAsync(memoryStream);

                    var document = new DocumentBD()
                    {
                        FileName = Path.GetFileName(file.FileName),
                        FileType = ext,
                        AccountId = accountId,
                        File = memoryStream.ToArray(),
                        UserId = userId,
                        CreatedAt = DateTime.Now.ToString(),
                    };
                    var result = await _unitOfWork.DocumentRepository.Add(document);
                    if (result is null) throw new Exception();
                }
                else
                {
                    return "The file is too large.";
                }
            }
            return "The upload sucess";
        }

        public async Task<byte[]> Get(int id, int userId)
        {
            var document = await _unitOfWork.DocumentRepository.Get(id);
            if (document is null) throw new ArgumentException("Document not found");
            if (document.UserId != userId) throw new AuthenticationException("You do not have permission to access this document");

            return document.File;
        }

        public async Task<List<DocumentResponse>> Get(int userId)
        {
            var documents = await _unitOfWork.DocumentRepository.GetAll(userId);
            if (documents is null) throw new ArgumentException("Document not found");



            var documentsResponse = new List<DocumentResponse>();
            foreach(var document in documents)
                documentsResponse.Add(MapDocumentResponse(document));

            return documentsResponse;
        }

        private static DocumentResponse MapDocumentResponse(DocumentBD document)
        {

            return new DocumentResponse()
            {
                Id = document.Id,
                FileName = document.FileName,
                Account_Id = document.AccountId,
                FileType = document.FileType,
            };
        }

    }
}
