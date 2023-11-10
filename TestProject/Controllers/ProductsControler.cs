using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.Data;
using TestProject.Domains;
using TestProject.Services.Interface;

namespace TestProject.Controllers;

public class ProductsController2 : Controller
{
    private readonly IProductRepository _productRepository;

    public ProductsController2(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    // Other actions...

    public async Task<IActionResult> Index()
    {
        var products = await _productRepository.GetAllProducts();
        return View(products);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var product = await _productRepository.GetProductByIdAsync(id.Value);
        if (product == null) return NotFound();

        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Quantity,Price")] Product product)
    {
        if (!ModelState.IsValid) return View(product);
        await _productRepository.CreateProductAsync(product);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Quantity,Price")] Product product)
    {
        if (id != product.Id) return NotFound();

        if (!ModelState.IsValid) return View(product);

        try
        {
            await _productRepository.UpdateProductAsync(product);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_productRepository.GetProductByIdAsync(product.Id) == null)
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _productRepository.DeleteProductAsync(id);
        if (product == null) return NotFound();

        return RedirectToAction(nameof(Index));
    }
}
