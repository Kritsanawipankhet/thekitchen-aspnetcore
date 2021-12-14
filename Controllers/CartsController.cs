using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using thekitchen_aspnetcore.Data;
using thekitchen_aspnetcore.Models;

namespace thekitchen_aspnetcore.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {

            _context = context;
            _userManager = userManager;
        }

        
        public async Task<IActionResult> Index()
        {
            ViewBag.action = "Cart";
            ViewBag.pageView = "Shopping Cart";
            var user = await _userManager.GetUserAsync(User);
            var applicationDbContext = _context.Carts.Include(c => c.Products).Include(c => c.Products.Categories).Where(c => c.User == user);
            var total = await _context.Carts.Include(c => c.Products).Where(i => i.UserID == user.Id).SumAsync(p => p.Quantity*p.Products.Price);
            if(total != null)
            {
                ViewBag.total = total;
            }
            if (user == null)
            {

                return View(await applicationDbContext.ToListAsync());
            }

            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> subQuantity(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var cart = await _context.Carts.FindAsync(id);
            if (id == null)
            {
                return RedirectToAction("Index", "Carts");
            }

            if (user.Id != cart.User.Id)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                if (cart.Quantity == 1)
                {
                    _context.Remove(cart);
                }
                else
                {
                    cart.Quantity -= 1;
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(cart.CartId))
                {
                    return RedirectToAction("Index", "Carts");
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Index", "Carts"); ;
        }

        public async Task<IActionResult> addQuantity(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var cart = await _context.Carts.FindAsync(id);
            if (id == null)
            {
                return RedirectToAction("Index", "Carts");
            }

            if (user.Id != cart.User.Id)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                cart.Quantity += 1;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(cart.CartId))
                {
                    return RedirectToAction("Index", "Carts");
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Index", "Carts"); ;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Products)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartId == id);
        }
    }
}
