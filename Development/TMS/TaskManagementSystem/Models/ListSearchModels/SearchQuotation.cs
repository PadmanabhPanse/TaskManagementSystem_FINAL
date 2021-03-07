using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models.ListSearchModels
{
    public class SearchQuotation
    {
        public string UserId { get; set; }
        public DateTime? QuotationDate { get; set; }
        public DateTime? ConversionDate { get; set; }
        public string Title { get; set; }
        public string QuoteStatus { get; set; }
        public decimal? TotalCost { get; set; }
    }
}
