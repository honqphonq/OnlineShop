using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        public readonly UserManager<IdentityUser> _userManager;
        public readonly ApplicationDbContext _context;
        public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.ApplicationUsers.ToList());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    TempData["save"] = "User has been created successfully";
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if(user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            var userInfor = _context.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if(userInfor == null)
            {
                return NotFound();
            }
            userInfor.FirstName = user.FirstName;
            userInfor.LastName = user.LastName;
            var result = await _userManager.UpdateAsync(userInfor);
            if (result.Succeeded)
            {
                TempData["save"] = "User has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(userInfor);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationUser user)
        {
            var userInfo = _context.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();

            }
            _context.ApplicationUsers.Remove(userInfo);
            int rowAffected = _context.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["save"] = "User has been delete successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }


        public async Task<IActionResult> Lockout(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Lockout(ApplicationUser user)
        {
            var userInfor = _context.ApplicationUsers.FirstOrDefault(u => u.Id == user.Id);
            if(userInfor == null)
            {
                return NotFound();
            }
            userInfor.LockoutEnd = DateTime.Now.AddYears(100);
            int rowAffected = _context.SaveChanges();
            if(rowAffected > 0)
            {
                TempData["save"] = "User has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(userInfor);
        }

        public async Task<IActionResult> Active(string id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if(user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Active(ApplicationUser user)
        {
            var userInfo = _context.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();

            }
            userInfo.LockoutEnd = DateTime.Now.AddDays(-1);
            int rowAffected = _context.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["save"] = "User has been active successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }


    }
}
