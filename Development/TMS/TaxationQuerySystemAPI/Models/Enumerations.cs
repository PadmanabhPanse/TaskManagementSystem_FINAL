using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TaxationQuerySystemAPI.Models
{
    public enum tmsRole 
    {
        Admin = 1,
        TaskManager = 2,
        TeamMember = 3,
        User = 4
    }
    public enum TaxTypeEnum
    {
        [XmlEnum(Name = "Income Tax")]
        IncomeTax = 1,
        [XmlEnum(Name = "GST")]
        GST = 2,
        [XmlEnum(Name = "Inter-Country Tax")]
        NationalTax = 3
    }
    public enum QueryType
    {
        [XmlEnum(Name = "Per Query")]
        PerQuery = 1, 
        [XmlEnum(Name = "Subscription")]
        Subscription = 2,
    }
}
