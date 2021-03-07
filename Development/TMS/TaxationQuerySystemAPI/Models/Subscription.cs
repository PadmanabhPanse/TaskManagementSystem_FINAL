using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TaxationQuerySystemAPI.Models
{
    public class Subscription
    {
        [XmlAttribute]
        public long Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlElement]
        public string taxType { get; set; }
        [XmlElement]
        public decimal taxRate { get; set; }
        [XmlElement]
        public QueryType querytype { get; set; }
        [XmlElement]
        public QueryRange queryRange { get; set; }
        [XmlElement]
        public decimal CostPerQuery { get; set; }
        [XmlElement]
        public int Credits { get; set; }
        [XmlElement]
        public decimal CostPerCredit { get; set; }
        [XmlElement]
        public decimal TotalCost { get; set; }
    }
}
