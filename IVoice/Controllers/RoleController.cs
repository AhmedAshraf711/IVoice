﻿using IVoice.Data;
using IVoice.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IVoice.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext  applicationDbContext;

        public RoleController(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext)
        {
            this.roleManager = roleManager;
            this.applicationDbContext = applicationDbContext;
        }
     
        public IActionResult Index()
        {
            ViewBag.Roles = applicationDbContext.Roles.ToList();
            return View();
        }
        [HttpGet]
      
        public IActionResult NewRole()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> NewRole(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole roleModel = new IdentityRole();
                roleModel.Name = roleViewModel.RoleName;
                //sv db
                IdentityResult result = await roleManager.CreateAsync(roleModel);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(roleViewModel);
        }


    }
}
