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
    public class LogoController : ControllerBase
    {
        public LogoManager _logoManager { get; }
        public LogoController(LogoManager logoManager)
        {
            _logoManager = logoManager;
        }

        [HttpGet]
        public ActionResult<Logo> GetLogos()
        {
            try
            {
                _logoManager.LoadXml();
                return Ok(_logoManager.xmlObjects);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult SaveLogos(Logo logo)
        {
            try
            {
                _logoManager.xmlObjects = logo;
                _logoManager.SaveXml();

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }
    }
}