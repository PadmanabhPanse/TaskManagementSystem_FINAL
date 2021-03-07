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
    [ApiController]
    public class SiteMapController : ControllerBase
    {
        public SiteMapManager _siteMapManager { get; }
        public SiteMapController(SiteMapManager siteMapManager)
        {
            _siteMapManager = siteMapManager;
        }

        #region "Menu"

        // GET: api/Menus
        [HttpGet("api/Menus")]
        public ActionResult<IEnumerable<Menu>> GetMenus()
        {
            try
            {
                _siteMapManager.LoadXml();
                return Ok(_siteMapManager.xmlObjects.OrderByDescending(o => o.Id));
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }

        }

        // GET: api/Menus/5
        [HttpGet("api/Menus/{id}")]
        public ActionResult<Menu> GetMenu(long id)
        {

            try
            {
                _siteMapManager.LoadXml();
                var menu = _siteMapManager.xmlObjects.FirstOrDefault(m => m.Id == id);

                if (menu == null)
                {
                    return NotFound(new { Error = $"Menu with id {id} not found" });
                }

                return Ok(menu);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        // PUT: api/Menus/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("api/Menus/{id}")]
        public IActionResult PutMenu(long id, Menu menu)
        {
            if (id != menu.Id)
            {
                return BadRequest();
            }
            if (!string.IsNullOrEmpty(menu.Roles) && !CheckRoleExist(menu.Roles.Split(',')))
            {
                return NotFound(new { Error = $"Invalid Roles {menu.Roles} Added" });
            }
            try
            {
                _siteMapManager.LoadXml();
                _siteMapManager.EditMenu(menu);
                _siteMapManager.SaveXml();
            }
            catch (Exception ex)
            {
                if (ex is System.IO.FileNotFoundException)
                {
                    return NotFound(new { Error = ex.Message });
                }
                if (!MenuExists(id))
                {
                    return NotFound(new { Error = $"Menu with id {id} not found" });
                }
                else
                {
                    return NotFound(new { Error = ex.Message });
                }
            }

            return Ok();
        }

        // POST: api/Menus
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("api/Menus")]
        public ActionResult<Menu> PostMenu(Menu menu)
        {
            try
            {
                if (!string.IsNullOrEmpty(menu.Roles) && !CheckRoleExist(menu.Roles.Split(',')))
                {
                    return NotFound(new { Error = $"Invalid Roles Added" });
                }
                if (menu.menuItems.Count > 0 && menu.menuItems.Any(item => !string.IsNullOrEmpty(item.Roles) && !CheckRoleExist(item.Roles.Split(','))))
                {
                    return NotFound(new { Error = $"Invalid Roles Added" });
                }
                _siteMapManager.LoadXml();
                _siteMapManager.AddMenu(menu);
                _siteMapManager.SaveXml();

                return Ok(menu);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        // DELETE: api/Menus/5
        [HttpDelete("api/Menus/{id}")]
        public ActionResult<Menu> DeleteMenu(long id)
        {
            try
            {
                _siteMapManager.LoadXml();
                var menu = _siteMapManager.xmlObjects.FirstOrDefault(m => m.Id == id);
                if (menu == null)
                {
                    return NotFound(new { Error = $"Menu with id {id} not found" });
                }

                _siteMapManager.RemoveMenu(menu);
                _siteMapManager.SaveXml();

                return Ok(menu);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        private bool MenuExists(long id)
        {
            return _siteMapManager.xmlObjects.Any(e => e.Id == id);
        }

        #endregion

        #region "MenuItems"
        // GET: api/MenuItems
        [HttpGet("api/MenuItems")]
        public ActionResult<IEnumerable<MenuItem>> GetMenuItems()
        {
            try
            {
                _siteMapManager.LoadXml();
                List<MenuItem> menuItems = new List<MenuItem>();
                menuItems = _siteMapManager.xmlObjects.SelectMany(menu => menu.menuItems).ToList();
                return Ok(menuItems);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        // GET: api/MenuItems/5
        [HttpGet("api/MenuItems/{id}")]
        public ActionResult<MenuItem> GetMenuItem(long id)
        {
            try
            {
                _siteMapManager.LoadXml();
                var menuItem = _siteMapManager.xmlObjects.SelectMany(menu => menu.menuItems).FirstOrDefault(item => item.Id == id);

                if (menuItem == null)
                {
                    return NotFound(new { Error = $"MenuItem with id {id} not found" });
                }

                return Ok(menuItem);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        // PUT: api/MenuItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("api/Menus/{parentMenuId}/MenuItems/{id}")]
        public IActionResult PutMenuItem(long id, long parentMenuId, MenuItem menuItem)
        {
            if (id != menuItem.Id)
            {
                return BadRequest();
            }
            if (!string.IsNullOrEmpty(menuItem.Roles) && !CheckRoleExist(menuItem.Roles.Split(',')))
            {
                return NotFound(new { Error = $"Invalid Roles {menuItem.Roles} Added" });
            }
            try
            {
                _siteMapManager.LoadXml();
                if (!MenuExists(parentMenuId))
                {
                    return NotFound(new { Error = $"Menu with id {parentMenuId} not found" });
                }
                _siteMapManager.EditMenuItem(parentMenuId, menuItem);
                _siteMapManager.SaveXml();
            }
            catch (Exception ex)
            {
                if (ex is System.IO.FileNotFoundException)
                {
                    return NotFound(new { Error = ex.Message });
                }
                if (!MenuItemExists(id))
                {
                    return NotFound(new { Error = $"MenuItem with id {id} not found" });
                }
                else
                {
                    return NotFound(new { Error = ex.Message });
                }
            }

            return Ok();
        }

        // POST: api/MenuItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("api/Menus/{parentMenuId}/MenuItems/")]
        public ActionResult<MenuItem> PostMenuItem(long parentMenuId, MenuItem menuItem)
        {
            try
            {
                if (!string.IsNullOrEmpty(menuItem.Roles) && !CheckRoleExist(menuItem.Roles.Split(',')))
                {
                    return NotFound(new { Error = $"Invalid Roles {menuItem.Roles} Added" });
                }
                _siteMapManager.LoadXml();
                if (!MenuExists(parentMenuId))
                {
                    return NotFound(new { Error = $"Menu with id {parentMenuId} not found" });
                }
                _siteMapManager.AddMenuItem(parentMenuId, menuItem);
                _siteMapManager.SaveXml();

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        // DELETE: api/MenuItems/5
        [HttpDelete("api/Menus/{parentMenuId}/MenuItems/{id}")]
        public ActionResult<MenuItem> DeleteMenuItem(long id, long parentMenuId)
        {
            try
            {
                _siteMapManager.LoadXml();
                if (!MenuExists(parentMenuId))
                {
                    return NotFound(new { Error = $"Menu with id {parentMenuId} not found" });
                }
                var menuItem = _siteMapManager.xmlObjects.SelectMany(menu => menu.menuItems).FirstOrDefault(item => item.Id == id);

                if (menuItem == null)
                {
                    return NotFound(new { Error = $"MenuItem with id {id} not found" });
                }

                _siteMapManager.RemoveMenuItem(parentMenuId, menuItem);
                _siteMapManager.SaveXml();

                return Ok(menuItem);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        private bool MenuItemExists(long id)
        {
            return _siteMapManager.xmlObjects.SelectMany(menu => menu.menuItems).Any(e => e.Id == id);
        }

        private bool CheckRoleExist(string[] roles)
        {
            return Enum.GetNames(typeof(tmsRole)).Intersect(roles.Select(r => r.Trim())).Count() == roles.Length;
        }

        // PUT: api/Menus/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("api/Menus/AssignRoles")]
        public IActionResult AssignRoles([FromBody]List<MenuItem> menuItems)
        {
            if (menuItems == null || menuItems?.Count() == 0)
            {
                return BadRequest();
            }
            //if (!string.IsNullOrEmpty(menu.Roles) && !CheckRoleExist(menu.Roles.Split(',')))
            //{
            //    return NotFound(new { Error = $"Invalid Roles {menu.Roles} Added" });
            //}
            try
            {
                _siteMapManager.LoadXml();

                foreach (var item in menuItems)
                {
                    _siteMapManager.EditMenuItem(item.ParentMenuId, item);
                }

                _siteMapManager.SaveXml();
            }
            catch (Exception ex)
            {
                //if (ex is System.IO.FileNotFoundException)
                //{
                //    return NotFound(new { Error = ex.Message });
                //}
                //if (!MenuExists(id))
                //{
                //    return NotFound(new { Error = $"Menu with id {id} not found" });
                //}
                //else
                //{
                return NotFound(new { Error = ex.Message });
                //   }
            }

            return Ok(menuItems);
        }
        #endregion
    }
}