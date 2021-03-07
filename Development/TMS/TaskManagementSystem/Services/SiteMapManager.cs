using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TaxationQuerySystem.HttpClient;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Services
{
    public class SiteMapManager
    {

        private readonly string ApiUrl = null;

        public IConfiguration _configuration { get; }

        public SiteMapManager(IConfiguration configuration)
        {
            _configuration = configuration;
            //this.ApiUrl = string.Concat(_configuration.GetValue<string>("TMSAPI:Url"), "/api");
            this.ApiUrl = $"{HttpRequestFactory.apihost}/api";
        }

        #region "Menu"

        public async Task<List<Menu>> GetMenus()
        {
            var resp = await HttpRequestFactory.Get(this.ApiUrl + "/Menus/");
            return resp.ContentAsType<List<Menu>>();
        }

        public async Task<Menu> GetMenu(long id)
        {
            var resp = await HttpRequestFactory.Get(this.ApiUrl + $"/Menus/{id}");
            return resp.ContentAsType<Menu>();
        }

        public async Task<System.Net.Http.HttpResponseMessage> PutMenu(long id, Menu menu)
        {
           return await HttpRequestFactory.Put(this.ApiUrl + $"/Menus/{id}", menu);
        }

        public async Task<System.Net.Http.HttpResponseMessage> PostMenu(Menu menu)
        {
            return await HttpRequestFactory.Post(this.ApiUrl + "/Menus/", menu);
        }

        public async Task<System.Net.Http.HttpResponseMessage> DeleteMenu(long id)
        {
            return await HttpRequestFactory.Delete(this.ApiUrl + $"/Menus/{id}");
        }

        #endregion

        #region "MenuItems"
        public async Task<List<MenuItem>> GetMenuItems()
        {
            var resp = await HttpRequestFactory.Get(this.ApiUrl + "/MenuItems/");
            return resp.ContentAsType<List<MenuItem>>();
        }

        public async Task<MenuItem> GetMenuItem(long id)
        {
            var resp = await HttpRequestFactory.Get(this.ApiUrl + $"/MenuItems/{id}");
            return resp.ContentAsType<MenuItem>();
        }

        public async Task<System.Net.Http.HttpResponseMessage> PutMenuItem(long id, long parentMenuId, MenuItem menuItem)
        {
            return await HttpRequestFactory.Put(this.ApiUrl + $"/Menus/{parentMenuId}/MenuItems/{id}", menuItem);
        }

        public async Task<System.Net.Http.HttpResponseMessage> PostMenuItem(long parentMenuId, MenuItem menuItem)
        {
            return await HttpRequestFactory.Post(this.ApiUrl + $"/Menus/{parentMenuId}/MenuItems/", menuItem);
        }

        public async Task<System.Net.Http.HttpResponseMessage> DeleteMenuItem(long id, long parentMenuId)
        {
            return await HttpRequestFactory.Delete(this.ApiUrl + $"/Menus/{parentMenuId}/MenuItems/{id}");
        }
        #endregion

        public async Task<List<MenuItem>> AssignRoles(List<MenuItem> menuItems)
        {
            var resp = await HttpRequestFactory.Put(this.ApiUrl + $"/Menus/AssignRoles", menuItems);
            return resp.ContentAsType<List<MenuItem>>();
        }

        

         
    }
}
