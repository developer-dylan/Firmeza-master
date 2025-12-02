namespace Firmeza.Web.Controllers;

using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Models.Entities;
using Firmeza.Web.ViewModels.Products;

[Authorize(Roles = "Admin")]
public class ProductsController : Controller
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productservice)
    {
        _productService = productservice;
    }

    // List paginated products
    public async Task<IActionResult> Index(int pageNumber = 1)
    {
        const int pageSize = 10;
        var products = await _productService.GetPagedProductsAsync(pageNumber, pageSize);
        return View(products);
    }

    // Display product details
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var product = await _productService.GetProductById(id.Value);

        if (product == null)
            return NotFound();

        return View(product);
    }

    // CREATE - GET
    public IActionResult Create()
    {
        return View();
    }

    // CREATE - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var product = new Product
        {
            Name = vm.Name!.Trim(),
            Price = vm.Price,
            Quantity = vm.Quantity,
            Category = vm.Category,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _productService.CreateProduct(product);

        if (created)
        {
            TempData["Success"] = "Product created successfully!";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, "An error occurred while creating the product.");
        return View(vm);
    }

    // EDIT - GET
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var product = await _productService.GetProductById(id.Value);
        if (product == null)
            return NotFound();

        var vm = new EditProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Quantity = product.Quantity,
            Category = product.Category
        };

        return View(vm);
    }

    // EDIT - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditProductViewModel vm)
    {
        if (id != vm.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(vm);

        var product = await _productService.GetProductById(id);
        if (product == null)
            return NotFound();

        product.Name = vm.Name!.Trim();
        product.Price = vm.Price;
        product.Quantity = vm.Quantity;
        product.Category = vm.Category;

        try
        {
            var updated = await _productService.UpdateProduct(product);
            if (updated)
            {
                TempData["Success"] = "Product updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "An error occurred while updating the product.");
            return View(vm);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(vm.Id))
                return NotFound();
            else
                throw;
        }
    }

    // DELETE - GET
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var product = await _productService.GetProductById(id.Value);
        if (product == null)
            return NotFound();

        return View(product);
    }

    // DELETE - POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var deleted = await _productService.DeleteProduct(id);
        if (deleted)
        {
            TempData["Success"] = "Product deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, "An error occurred while deleting the product.");
        var product = await _productService.GetProductById(id);
        return View(product);
    }

    private bool ProductExists(int id)
    {
        var product = _productService.GetProductById(id).Result;
        return product != null;
    }
}
