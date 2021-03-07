using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxationQuerySystemAPI.Models
{
    public class QuoteTask
    {
        public long TaskId { get; set; }
        public long QuoteId { get; set; }
        public string TaskName { get; set; }
        public string TaskTitle { get; set; }
        public decimal Cost { get; set; }
        public Quotation Quotation { get; set; }
    }
}
