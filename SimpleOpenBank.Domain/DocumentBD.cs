using SimpleOpenBank.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Domain
{
    public class DocumentBD : BaseDomainEntity
    {
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] File { get; set; }
        public string CreatedAt { get; set; }

    }
}
