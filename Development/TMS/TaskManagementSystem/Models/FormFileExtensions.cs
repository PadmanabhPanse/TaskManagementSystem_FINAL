using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models
{
    public static class FormFileExtensions
    {
        public static async Task<byte[]> getBytesAsync(this Microsoft.AspNetCore.Http.IFormFile formFile)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static async Task<string> getBase64StringAsync(this Microsoft.AspNetCore.Http.IFormFile formFile)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        public static byte[] getBytes(this Microsoft.AspNetCore.Http.IFormFile formFile)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static string getBase64String(this Microsoft.AspNetCore.Http.IFormFile formFile)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }

}
