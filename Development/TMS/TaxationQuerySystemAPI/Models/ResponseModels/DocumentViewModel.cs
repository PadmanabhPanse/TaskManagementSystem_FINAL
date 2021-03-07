using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxationQuerySystemAPI.Models.ResponseModels
{
    public class DocumentViewModel
    {
        [System.ComponentModel.DataAnnotations.Key]
        public long DocumentId { get; set; }
        public DateTime DocumentDate { get; set; }
        public long DocumentOwnerId { get; set; }
        public string DocumentComments { get; set; }
        public long DocumentTaskId { get; set; }
        public string filename { get; set; }
        public string file { get; set; }
        public string DocumentPhysicalPath { get; set; }
        public long? DocumentKeywordId { get; set; }
        public Microsoft.EntityFrameworkCore.EntityState ChangeState { get; set; }
    }
}
