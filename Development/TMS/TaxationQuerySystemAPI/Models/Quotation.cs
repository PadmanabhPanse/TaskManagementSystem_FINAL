using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxationQuerySystemAPI.Models
{
    public class Quotation
    {
        public long QuoteId { get; set; }
        public string UserId { get; set; }
        public DateTime QuotationDate { get; set; }
        public DateTime ConversionDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string QuoteStatus { get; set; }
        public string RejectReason { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public decimal TotalCost { get; set; }
        public string TaxType { get; set; }
        public decimal TaxRate { get; set; }
        public List<QuoteTask> tasks { get; set; }
    }
}
