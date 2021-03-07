using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxationQuerySystemAPI.Models;
using TaxationQuerySystemAPI.Services;

namespace TaxationQuerySystemAPI.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPageController : ControllerBase
    {
        public BlogPageManager _blogPageManager { get; }
        public BlogPageController(BlogPageManager blogPageManager)
        {
            _blogPageManager = blogPageManager;
        }
        [HttpGet]
        public ActionResult<Blog> GetContents()
        {
            try
            {
                _blogPageManager.LoadXml();
                return Ok(_blogPageManager.xmlObjects);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult SaveContents(Blog blog)
        {
            try
            {
                _blogPageManager.xmlObjects = blog;
                _blogPageManager.SaveXml();

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

    }
}
