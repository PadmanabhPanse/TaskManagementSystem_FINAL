using System;
using System.Collections.Generic;

namespace TaxationQuerySystemAPI.Models
{
    public partial class Document
    {
        public long DocumentId { get; set; }
        public DateTime DocumentDate { get; set; }
        public long DocumentOwnerId { get; set; }
        public string DocumentComments { get; set; }
        public long DocumentTaskId { get; set; }
        public string DocumentPhysicalPath { get; set; }
        public long? DocumentKeywordId { get; set; }

        public Task DocumentTask { get; set; }
    }
}
