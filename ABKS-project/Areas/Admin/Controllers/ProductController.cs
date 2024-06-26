using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using System.Threading.Tasks;
using ABKS_project.Areas.Ecommerce.Models;
using Microsoft.AspNetCore.Http;
using ABKS_project.Areas.Ecommerce.ViewModels;

namespace ABKS_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly productContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(productContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> ListProduct(int pageNumber = 1, int pageSize = 4, string search = null)
        {
            var productsQuery = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                productsQuery = productsQuery.Where(p => p.ProductName.Contains(search) || p.ProductCategory.CategoryName.Contains(search));
            }

            var totalCount = await productsQuery.CountAsync();

            var products = await productsQuery
                .Include(p => p.ProductCategory)
                .OrderBy(p => p.ProductName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.CurrentFilter = search;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(products);
        }

        public IActionResult AddProduct()
        {
            var categories = _context.ProductCategories.ToList();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string fileName = "";

                if (model.Photo != null)
                {
                    string folder = Path.Combine(_env.WebRootPath, "Images/Products");
                    fileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Photo.CopyToAsync(stream);
                    }

                    var product = new Product
                    {
                        ProductName = model.ProductName,
                        ProductPrice = (double)model.ProductPrice,
                        ProductDescription = model.ProductDescription,
                        ProductImg = fileName,
                        ProductCategoryId = model.ProductCategoryId,
                        InStock = model.InStock
                    };

                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ListProduct));
                }
            }
            var categories = _context.ProductCategories.ToList();
            ViewBag.Categories = categories;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var product = await _context.Products
                .Include(p => p.OrderDetails)  // Include related OrderDetails
                .Include(p => p.CartDetails)   // Include related CartDetails
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return NotFound();
            }

            // Delete related OrderDetails first
            if (product.OrderDetails != null && product.OrderDetails.Any())
            {
                _context.OrderDetails.RemoveRange(product.OrderDetails);
            }

            // Delete related CartDetails first
            if (product.CartDetails != null && product.CartDetails.Any())
            {
                _context.CartDetails.RemoveRange(product.CartDetails);
            }

            // Delete the product image file if it exists
            if (!string.IsNullOrEmpty(product.ProductImg))
            {
                string filePath = Path.Combine(_env.WebRootPath, "Images/Products", product.ProductImg);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // Remove the product from the database
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ListProduct));
        }


 
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new EditProductViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductPrice = (decimal)product.ProductPrice,
                ProductDescription = product.ProductDescription,
                ExistingPhotoPath = product.ProductImg,
                ProductCategoryId = product.ProductCategoryId,
                InStock = (bool)product.InStock
            };

            var categories = _context.ProductCategories.ToList();
            ViewBag.Categories = categories;
            return View(viewModel); // Use EditProduct view
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(EditProductViewModel model)
        {
            
                var product = await _context.Products.FindAsync(model.ProductId);
                if (product == null)
                {
                    return NotFound();
                }

                product.ProductName = model.ProductName;
                product.ProductPrice = (double)model.ProductPrice;
                product.ProductDescription = model.ProductDescription;
                product.ProductCategoryId = model.ProductCategoryId;
                product.InStock = model.InStock;

                if (model.Photo != null)
                {
                    string folder = Path.Combine(_env.WebRootPath, "Images/Products");
                    string fileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Photo.CopyToAsync(stream);
                    }

                    if (!string.IsNullOrEmpty(model.ExistingPhotoPath))
                    {
                        string existingFilePath = Path.Combine(folder, model.ExistingPhotoPath);
                        if (System.IO.File.Exists(existingFilePath))
                        {
                            System.IO.File.Delete(existingFilePath);
                        }
                    }

                    product.ProductImg = fileName;
                }

                _context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListProduct));
        }



    }
}
