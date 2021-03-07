using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models.ListSearchModels
{
    public static class QueryStringBuilder
    {
        public static string ToQueryString(this object obj)
        {

            return string.Join("&", obj.GetType()
                                       .GetProperties()
                                       .Where(p => p.GetValue(obj, null) != null) //Attribute.IsDefined(p, typeof(QueryStringAttribute)) && 
                                       .Select(p => $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(p.GetValue(obj).ToString())}"));
        }
    }
}
