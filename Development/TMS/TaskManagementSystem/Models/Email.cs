using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models
{
    public class Email
    {
        public long Id { get; set; }
        public string Types { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
