using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models
{
    public class Document
    {
        public long DocumentId { get; set; }
        public DateTime DocumentDate { get; set; }
        public long DocumentOwnerId { get; set; }
        public string DocumentComments { get; set; }
        public long DocumentTaskId { get; set; }
        public IFormFile formFile { get; set; }
        public string filename { get; set; }
        public string file { get; set; } //base64 file
        public string DocumentPhysicalPath { get; set; }
        public long? DocumentKeywordId { get; set; }
        public Microsoft.EntityFrameworkCore.EntityState ChangeState { get; set; }
    }
}
