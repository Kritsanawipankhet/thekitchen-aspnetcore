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
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index(int? id)
        {
            var applicationDbContext = _context.Products.Include(p => p.Categories);
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var product = await applicationDbContext.Where(m => m.ProductId == id).FirstOrDefaultAsync();
            if (product == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.action = "Product";
            ViewBag.pageView = product.Name;
            return View(product);
        }

        public async Task<IActionResult> addProductToCart(int? id)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Carts");
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var cartAvaliable = await _context.Carts.Where(i => i.UserID == user.Id).Where(p => p.ProductId == product.ProductId).FirstOrDefaultAsync();

            if (cartAvaliable != null)
            {
                cartAvaliable.Quantity += 1;
            }
            else
            {
                var cart = new Cart
                {

                    UserID = user.Id,
                    ProductId = (int)id,
                    Quantity = 1

                };
                _context.Add(cart);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Carts"); ;
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ProductManage()
        {
            var applicationDbContext = _context.Products.Include(p => p.Categories);

            var product = await applicationDbContext.ToListAsync();

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ProductManageCreate()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategotyId", "Name");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductManageAdd([Bind("ProductId,Name,ImageFile,Description,Price,Amount,Timestamp,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {

                string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                string extension = Path.GetExtension(product.ImageFile.FileName);
                product.Image = fileName = fileName + DateTime.UtcNow.ToString("yymmssfff") + extension;
                string path = Path.Combine(_hostEnvironment.WebRootPath + "/img-products", fileName);
                using(var fileStream = new FileStream(path, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(fileStream);

                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("ProductManage", "Products");
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategotyId", "Name", product.CategoryId);
            return RedirectToAction("ProductManage","Products");
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> ProductManageEdit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("ProductManage", "Products");
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction("ProductManage", "Products");
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategotyId", "Name", product.CategoryId);
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductManageEdit(int id, [Bind("ProductId,Name,ImageFile,Description,Price,Amount,Timestamp,CategoryId")] Product product)
        {
            if (id != product.ProductId)
            {
                return RedirectToAction("ProductManage", "Products");
            }

         

            if (ModelState.IsValid)
            {
                try { 
                   
                    string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                    string extension = Path.GetExtension(product.ImageFile.FileName);
                    product.Image = fileName = fileName + DateTime.UtcNow.ToString("yymmssfff") + extension;
                    string path = Path.Combine(_hostEnvironment.WebRootPath + "/img-products", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);

                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return RedirectToAction("ProductManage", "Products");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ProductManage", "Products");
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategotyId", "Name", product.CategoryId);
            return RedirectToAction("ProductManage", "Products");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ProductManageDeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return RedirectToAction("ProductManage", "Products");
            }
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "/img-products", product.Image);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("ProductManage", "Products");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
