using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models
{
    public enum Role
    {
        Admin = 1,
        TaskManager = 2,
        TeamMember = 3,
        User = 4
    }
    public enum TaxTypeEnum
    {
        [Display(Name = "Income Tax")]
        IncomeTax = 1,
        GST = 2,
        [Display(Name = "Inter-Country Tax")]
        NationalTax = 3
    }
    public enum QueryType
    {
        [Display(Name = "Per Query")]
        PerQuery = 1,
        Subscription = 2,
    }
}
