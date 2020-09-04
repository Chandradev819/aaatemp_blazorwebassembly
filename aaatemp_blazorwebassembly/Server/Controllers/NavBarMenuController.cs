using aaatemp_blazorwebassembly.Server.Data;
using aaatemp_blazorwebassembly.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aaatemp_blazorwebassembly.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class NavBarMenuController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;
        private readonly ApplicationDbContext context;

        public NavBarMenuController(IAuthorizationService authorizationService, ApplicationDbContext context)
        {
            this.authorizationService = authorizationService;
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<NavBarMenuItem> Get()
        {
            List<NavBarMenuItem> lstMenuItems = new List<NavBarMenuItem>();
            //return lstMenuItems.ToArray();
            var lstMenuItemsFromDb = context.NavBarMenuItems.OrderBy(m => m.ItemOrder).ToList();

            var parentItems = lstMenuItemsFromDb.Where(m => m.ParentMenuId == 0).OrderBy(m => m.ItemOrder).ToList();

            foreach (NavBarMenuItem mm in parentItems)
            {
                var result = authorizationService.AuthorizeAsync(HttpContext.User, mm.UserPolicy).Result;
                if (result.Succeeded)
                {
                    lstMenuItems.Add(mm);

                    var level1Items = lstMenuItemsFromDb.Where(m => m.ParentMenuId == mm.MenuId).OrderBy(m => m.ItemOrder).ToList();

                    foreach (NavBarMenuItem mm1 in level1Items)
                    {
                        var result1 = authorizationService.AuthorizeAsync(HttpContext.User, mm1.UserPolicy).Result;
                        if (result1.Succeeded)
                        {
                            mm.ChildItems.Add(mm1);

                            var level2Items = lstMenuItemsFromDb.Where(m => m.ParentMenuId == mm1.MenuId).OrderBy(m => m.ItemOrder).ToList();

                            foreach (NavBarMenuItem mm2 in level2Items)
                            {
                                var result2 = authorizationService.AuthorizeAsync(HttpContext.User, mm2.UserPolicy).Result;
                                if (result1.Succeeded)
                                {
                                    mm1.ChildItems.Add(mm2);
                                }
                            }
                        }
                    }
                }
            }

            return lstMenuItems.ToArray();
        }
    }
}
