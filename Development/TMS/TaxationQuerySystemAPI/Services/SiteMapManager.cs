using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TaxationQuerySystemAPI.Models;

namespace TaxationQuerySystemAPI.Services
{
    public class SiteMapManager : XmlFileHandler<List<Menu>>
    {

        //public List<Menu> menus = new List<Menu>();

        public SiteMapManager(IHostingEnvironment environment) : base(environment, "ArrayOfMenu", "sitemap.xml")
        {
        }

        #region Menus
        public void AddMenu(Menu newMenu)
        {
            xmlObjects.Add(newMenu);
        }

        public void EditMenu(Menu menu)
        {
            int index = xmlObjects.FindIndex(m => m.Id == menu.Id);
            Menu[] menuArr = xmlObjects.ToArray();
            menuArr[index] = menu;
            xmlObjects = menuArr.ToList();
        }

        public void RemoveMenu(Menu menu)
        {
            xmlObjects.Remove(menu);
        }
        public void AddMenuItem(long parentMenuId, MenuItem menuItem)
        {
            int parentMenuIndex = xmlObjects.FindIndex(m => m.Id == parentMenuId);
            Menu[] menuArr = xmlObjects.ToArray();
            Menu parentMenu = menuArr[parentMenuIndex];
            parentMenu.menuItems.Add(menuItem);
            menuArr[parentMenuIndex] = parentMenu;
            xmlObjects = menuArr.ToList();
        }

        public void EditMenuItem(long parentMenuId, MenuItem menuItem)
        {
            int parentMenuIndex = xmlObjects.FindIndex(m => m.Id == parentMenuId);
            Menu[] menuArr = xmlObjects.ToArray();
            Menu parentMenu = menuArr[parentMenuIndex];

            int menuItemIndex = parentMenu.menuItems.FindIndex(m => m.Id == menuItem.Id);
            MenuItem[] itemArr = parentMenu.menuItems.ToArray();
            itemArr[menuItemIndex] = menuItem;
            parentMenu.menuItems = itemArr.ToList();
            menuArr[parentMenuIndex] = parentMenu;
            xmlObjects = menuArr.ToList();
        }

        public void RemoveMenuItem(long parentMenuId, MenuItem menuItem)
        {
            int parentMenuIndex = xmlObjects.FindIndex(m => m.Id == parentMenuId);
            Menu[] menuArr = xmlObjects.ToArray();
            Menu parentMenu = menuArr[parentMenuIndex];
            parentMenu.menuItems.Remove(menuItem);
            menuArr[parentMenuIndex] = parentMenu;
            xmlObjects = menuArr.ToList();
        }
        #endregion

    }
}
