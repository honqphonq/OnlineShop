using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.Areas.Admin.Models;
using OnlineShop.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public RoleController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles;
            return View();
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            IdentityRole role = new IdentityRole();
            role.Name = name;
            var isExits = await _roleManager.RoleExistsAsync(role.Name);
            if (isExits)
            {
                ViewBag.msg = "This role is already exist!";
                ViewBag.name = name;
                return View();
            }
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                TempData["save"] = "Role has been created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if(role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if(role == null)
            {
                return NotFound();
            }
            role.Name = name;
            var isExits = await _roleManager.RoleExistsAsync(role.Name);
            if (isExits)
            {
                ViewBag.msg = "This role is already exist!";
                ViewBag.name = name;
                return View();
            }
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                TempData["save"] = "Role has been updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View();
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["delete"] = "Role has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Assign()
        {
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers.Where(f=>f.LockoutEnd < DateTime.Now || f.LockoutEnd == null).ToList(),"Id","UserName");
            ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(),"Name","Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Assign(RoleUserVM roleUser)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(c => c.Id == roleUser.UserId);
            var isCheckRoleAssign = await _userManager.IsInRoleAsync(user, roleUser.RoleId);
            if (isCheckRoleAssign)
            {
                ViewBag.msg = "This user already assign this role";
                ViewData["UserId"] = new SelectList(_context.ApplicationUsers.Where(f => f.LockoutEnd < DateTime.Now || f.LockoutEnd == null).ToList(), "Id", "UserName");
                ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
                return View();
            }
            var role = await _userManager.AddToRoleAsync(user, roleUser.RoleId);
            if (role.Succeeded)
            {
                TempData["save"] = "User Role has been assigned successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult AssignUserRole()
        {
            var result = from ur in _context.UserRoles
                         join r in _context.Roles
                         on ur.RoleId equals r.Id
                         join a in _context.ApplicationUsers
                         on ur.UserId equals a.Id
                         select new UserRoleMapping
                         {
                             UserId = ur.UserId,
                             RoleId = ur.RoleId,
                             UserName = a.UserName ,
                             RoleName = r.Name
                         };
            ViewBag.UserRoles = result;
            return View();
        }

    }
}
