using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models
{
     public class DocumentViewModel
    {
        public long DocumentId { get; set; }
        public DateTime DocumentDate { get; set; }
        public long DocumentOwnerId { get; set; }
        public string DocumentComments { get; set; }
        public long DocumentTaskId { get; set; }
        public IFormFile file { get; set; } 
        public string DocumentPhysicalPath { get; set; }
        public Microsoft.EntityFrameworkCore.EntityState ChangeStatus { get; set; }
        public long? DocumentKeywordId { get; set; }
    }
}
