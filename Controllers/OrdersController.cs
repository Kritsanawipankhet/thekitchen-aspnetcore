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

namespace thekitchen_aspnetcore
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;


        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {

            _context = context;
            _userManager = userManager;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            
            return View();
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        public async Task<IActionResult> CheckOutOrder()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var applicationDbContext = _context.Carts.Include(c => c.Products).Include(c => c.Products.Categories).Where(c => c.User == user);
            var total = await _context.Carts.Include(c => c.Products).Where(i => i.UserID == user.Id).SumAsync(p => p.Quantity * p.Products.Price);
            if (total == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.total = total;
            ViewData["Firstname"] = user.Firstname;
            ViewData["Lastname"] = user.Lastname;
            ViewData["PhoneNumber"] = user.PhoneNumber;
            ViewData["Email"] = user.Email;

            return View();
        }
        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,UserID,OrderReceiverFirstname,OrderReceiverLastname,OrderReceiverPhone,OrderReceiverAddress,OrderReceiverZipcode,OrderReceiverProvince,OrderDeliveryName,OrderDeliveryPrice,OrderTotal,OrderPaymentReport,OrderDeliveryTrack,OrderStatus,Timestamp")] Order order)
        {
            //if (ModelState.IsValid)
            //{
            //    var user = await _userManager.GetUserAsync(User);
            //    if (user == null)
            //    {
            //        return RedirectToAction("Index", "Home");
            //    }

            //    _context.Add(order);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction("Index", "Orders");
            //}
            //ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", order.UserID);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var cart = await _context.Carts.Where(c => c.User == user).FirstAsync();
            if (cart == null)
            {
                return RedirectToAction("Index", "Orders");
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Orders");
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", order.UserID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,UserID,OrderReceiverFirstname,OrderReceiverLastname,OrderReceiverPhone,OrderReceiverAddress,OrderReceiverZipcode,OrderReceiverProvince,OrderDeliveryName,OrderDeliveryPrice,OrderTotal,OrderPaymentReport,OrderDeliveryTrack,OrderStatus,Timestamp")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", order.UserID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
