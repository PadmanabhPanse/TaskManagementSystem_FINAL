using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxationQuerySystemAPI.Models.ResponseModels
{
    public class StaffIncentiveViewModel
    {
        public long IncetiveId { get; set; }
        public long StaffUserId { get; set; }
        public long AssignedBy { get; set; }
        public DateTime AssignedDate { get; set; }
        public long TaskId { get; set; }
        public decimal IncentivesDecided { get; set; }
        public decimal IncentivesPaid { get; set; }
        public DateTime IncentivesPaidDate { get; set; }
        public EntityState ChangeState { get; set; }
    }
}

