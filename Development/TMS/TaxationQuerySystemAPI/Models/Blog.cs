using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxationQuerySystemAPI.Models
{
    public class Blog
    {
        public Text BlogText { get; set; }
        public Text LeftText { get; set; }
        public Text RightText { get; set; }
        public Text FooterText { get; set; }
    }
}
