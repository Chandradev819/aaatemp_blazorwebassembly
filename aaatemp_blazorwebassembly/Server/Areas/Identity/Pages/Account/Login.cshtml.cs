using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using aaatemp_blazorwebassembly.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using aaatemp_blazorwebassembly.Server.Data;
using Microsoft.EntityFrameworkCore;
using aaatemp_blazorwebassembly.Shared.Models;

namespace aaatemp_blazorwebassembly.Server.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public LoginModel(SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
            this._context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public async Task OnPostCreateLogins()
        {
            //var info = await _signInManager.GetExternalLoginInfoAsync();

            if (!await _roleManager.RoleExistsAsync("BasicUser"))
                await _roleManager.CreateAsync(new IdentityRole("BasicUser"));
            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await _roleManager.RoleExistsAsync("Superadmin"))
                await _roleManager.CreateAsync(new IdentityRole("Superadmin"));

            var user = await _userManager.FindByNameAsync("user1@aaa.aaa");
            //var result = await _userManager.CreateAsync(user);
            //var result = await _userManager.AddLoginAsync(user, info);
            if (user == null)
            {
                ApplicationUser au = new ApplicationUser()
                {
                    UserName = "user1@aaa.aaa", Email = "user1@aaa.aaa"
                };

                var result = await _userManager.CreateAsync(au, "abCD$$123");
                user = await _userManager.FindByNameAsync("user1@aaa.aaa");
            }
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, "BasicUser");
                result = await _userManager.AddToRoleAsync(user, "Admin");
                result = await _userManager.AddToRoleAsync(user, "Superadmin");
                result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("BasicUser_StartDate", "01/01/2000"));
                result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("BasicUser_EndDate", "01/01/2070"));
                result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Admin_StartDate", "01/01/2000"));
                result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Admin_EndDate", "01/01/2070"));
                result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Superadmin_StartDate", "01/01/2000"));
                result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Superadmin_EndDate", "01/01/2070"));
            }

            user = await _userManager.FindByNameAsync("user2@aaa.aaa");
            if (user == null)
            {
                ApplicationUser au = new ApplicationUser()
                {
                    UserName = "user2@aaa.aaa",
                    Email = "user2@aaa.aaa"
                };

                var result = await _userManager.CreateAsync(au, "abCD$$123");
                user = await _userManager.FindByNameAsync("user2@aaa.aaa");
            }
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, "BasicUser");
                result = await _userManager.AddToRoleAsync(user, "Admin");
                result = await _userManager.AddToRoleAsync(user, "Superadmin");
                result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("BasicUser_StartDate", "01/01/2000"));
                result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("BasicUser_EndDate", "01/01/2070"));
                result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Admin_StartDate", "01/01/2000"));
                result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Admin_EndDate", "01/01/2070"));
                result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Superadmin_StartDate", "01/01/2000"));
                result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Superadmin_EndDate", "01/01/2070"));
            }

            user = await _userManager.FindByNameAsync("user3@aaa.aaa");
            if (user == null)
            {
                ApplicationUser au = new ApplicationUser()
                {
                    UserName = "user3@aaa.aaa",
                    Email = "user3@aaa.aaa"
                };

                var result = await _userManager.CreateAsync(au, "abCD$$123");
                user = await _userManager.FindByNameAsync("user3@aaa.aaa");
            }
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, "BasicUser");
                result = await _userManager.AddToRoleAsync(user, "Admin");
                result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("BasicUser_StartDate", "01/01/2000"));
                result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("BasicUser_EndDate", "01/01/2070"));
                result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Admin_StartDate", "01/01/2000"));
                result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Admin_EndDate", "01/01/2070"));
            }

            user = await _userManager.FindByNameAsync("user4@aaa.aaa");
            if (user == null)
            {
                ApplicationUser au = new ApplicationUser()
                {
                    UserName = "user4@aaa.aaa",
                    Email = "user4@aaa.aaa"
                };

                var result = await _userManager.CreateAsync(au, "abCD$$123");
                user = await _userManager.FindByNameAsync("user4@aaa.aaa");
            }
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, "BasicUser");
                result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("BasicUser_StartDate", "01/01/2000"));
                result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("BasicUser_EndDate", "01/01/2070"));
            }

            //----------------------------------------------------------

            _context.Database.ExecuteSqlRaw("TRUNCATE TABLE NavBarMenuItems");

            #region Parent Items

            int menuItemOrder = 0;

            NavBarMenuItem mmMyInfo = new NavBarMenuItem()
            {
                MenuDisplayName = "My Info",
                ParentMenuId = 0,
                UserPolicy = "BasicUserPolicy",
                MenuURL = "",
                CreatedDate = DateTime.Now,
                ItemOrder = menuItemOrder++
            };

            _context.NavBarMenuItems.Add(mmMyInfo);
            _context.SaveChanges();

            NavBarMenuItem mmAdmin = new NavBarMenuItem()
            {
                MenuDisplayName = "Admin",
                ParentMenuId = 0,
                UserPolicy = "AdminPolicy",
                MenuURL = "",
                CreatedDate = DateTime.Now,
                ItemOrder = menuItemOrder++
            };

            _context.NavBarMenuItems.Add(mmAdmin);
            _context.SaveChanges();

            NavBarMenuItem mmSuperadmin = new NavBarMenuItem()
            {
                MenuDisplayName = "Superadmin",
                ParentMenuId = 0,
                UserPolicy = "SuperadminPolicy",
                MenuURL = "",
                CreatedDate = DateTime.Now,
                ItemOrder = menuItemOrder++
            };

            _context.NavBarMenuItems.Add(mmSuperadmin);
            _context.SaveChanges();

            #endregion

            #region First Level Items

            menuItemOrder = 0;
            NavBarMenuItem mmBasicSubItem1 = new NavBarMenuItem()
            {
                MenuDisplayName = "BasicSubItem1",
                ParentMenuId = mmMyInfo.MenuId,
                UserPolicy = "BasicUserPolicy",
                MenuURL = "",
                CreatedDate = DateTime.Now,
                ItemOrder = menuItemOrder++
            };

            _context.NavBarMenuItems.Add(mmBasicSubItem1);
            _context.SaveChanges();

            NavBarMenuItem mmBasicSubItem2 = new NavBarMenuItem()
            {
                MenuDisplayName = "BasicSubItem2",
                ParentMenuId = mmMyInfo.MenuId,
                UserPolicy = "BasicUserPolicy",
                MenuURL = "",
                CreatedDate = DateTime.Now,
                ItemOrder = menuItemOrder++
            };

            _context.NavBarMenuItems.Add(mmBasicSubItem2);
            _context.SaveChanges();

            NavBarMenuItem mmBasicSubItem3 = new NavBarMenuItem()
            {
                MenuDisplayName = "BasicSubItem3",
                ParentMenuId = mmMyInfo.MenuId,
                UserPolicy = "BasicUserPolicy",
                MenuURL = "",
                CreatedDate = DateTime.Now,
                ItemOrder = menuItemOrder++
            };

            _context.NavBarMenuItems.Add(mmBasicSubItem3);
            _context.SaveChanges();

            //Admin
            menuItemOrder = 0;
            NavBarMenuItem mm = new NavBarMenuItem()
            {
                MenuDisplayName = "AdminSubItem1",
                ParentMenuId = mmAdmin.MenuId,
                UserPolicy = "AdminPolicy",
                MenuURL = "",
                CreatedDate = DateTime.Now,
                ItemOrder = menuItemOrder++
            };

            _context.NavBarMenuItems.Add(mm);
            _context.SaveChanges();

            mm = new NavBarMenuItem()
            {
                MenuDisplayName = "AdminSubItem2",
                ParentMenuId = mmAdmin.MenuId,
                UserPolicy = "AdminPolicy",
                MenuURL = "",
                CreatedDate = DateTime.Now,
                ItemOrder = menuItemOrder++
            };

            _context.NavBarMenuItems.Add(mm);
            _context.SaveChanges();

            mm = new NavBarMenuItem()
            {
                MenuDisplayName = "AdminSubItem3",
                ParentMenuId = mmAdmin.MenuId,
                UserPolicy = "AdminPolicy",
                MenuURL = "",
                CreatedDate = DateTime.Now,
                ItemOrder = menuItemOrder++
            };

            _context.NavBarMenuItems.Add(mm);
            _context.SaveChanges();

            //Superadmin
            menuItemOrder = 0;
            mm = new NavBarMenuItem()
            {
                MenuDisplayName = "SuperadminSubItem1",
                ParentMenuId = mmSuperadmin.MenuId,
                UserPolicy = "SuperadminPolicy",
                MenuURL = "",
                CreatedDate = DateTime.Now,
                ItemOrder = menuItemOrder++
            };

            _context.NavBarMenuItems.Add(mm);
            _context.SaveChanges();

            mm = new NavBarMenuItem()
            {
                MenuDisplayName = "SuperadminSubItem2",
                ParentMenuId = mmSuperadmin.MenuId,
                UserPolicy = "SuperadminPolicy",
                MenuURL = "",
                CreatedDate = DateTime.Now,
                ItemOrder = menuItemOrder++
            };

            _context.NavBarMenuItems.Add(mm);
            _context.SaveChanges();

            mm = new NavBarMenuItem()
            {
                MenuDisplayName = "SuperadminSubItem3",
                ParentMenuId = mmSuperadmin.MenuId,
                UserPolicy = "SuperadminPolicy",
                MenuURL = "",
                CreatedDate = DateTime.Now,
                ItemOrder = menuItemOrder++
            };

            _context.NavBarMenuItems.Add(mm);
            _context.SaveChanges();

            #endregion

            #region Second Level Items

            menuItemOrder = 0;
            mm = new NavBarMenuItem()
            {
                MenuDisplayName = "BasicSub-SubItem1",
                ParentMenuId = mmBasicSubItem1.MenuId,
                UserPolicy = "BasicUserPolicy",
                MenuURL = "",
                CreatedDate = DateTime.Now,
                ItemOrder = menuItemOrder++
            };

            _context.NavBarMenuItems.Add(mm);
            _context.SaveChanges();

            mm = new NavBarMenuItem()
            {
                MenuDisplayName = "BasicSub-SubItem2",
                ParentMenuId = mmBasicSubItem1.MenuId,
                UserPolicy = "BasicUserPolicy",
                MenuURL = "",
                CreatedDate = DateTime.Now,
                ItemOrder = menuItemOrder++
            };

            _context.NavBarMenuItems.Add(mm);
            _context.SaveChanges();

            mm = new NavBarMenuItem()
            {
                MenuDisplayName = "BasicSub-SubItem3",
                ParentMenuId = mmBasicSubItem1.MenuId,
                UserPolicy = "BasicUserPolicy",
                MenuURL = "",
                CreatedDate = DateTime.Now,
                ItemOrder = menuItemOrder++
            };

            _context.NavBarMenuItems.Add(mm);
            _context.SaveChanges();

            mm = new NavBarMenuItem()
            {
                MenuDisplayName = "BasicSub-SubItem1",
                ParentMenuId = mmBasicSubItem1.MenuId,
                UserPolicy = "BasicUserPolicy",
                MenuURL = "",
                CreatedDate = DateTime.Now,
                ItemOrder = menuItemOrder++
            };

            _context.NavBarMenuItems.Add(mm);
            _context.SaveChanges();


            #endregion







            int iii = 0;
        }
    }
}
