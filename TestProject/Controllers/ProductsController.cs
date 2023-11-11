using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.Domains;
using TestProject.Services.Interfaces;

namespace TestProject.Controllers;

[Authorize]
public class ProductsController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly UserManager<User> _userManager;

    public ProductsController(IProductRepository productRepository, UserManager<User> userManager)
    {
        _productRepository = productRepository;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productRepository.GetAllProducts();
        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        if (id == null) return NotFound();

        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null) return NotFound();

        return View(product);
    }

    public IActionResult Create() => View();

    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Quantity,Price")] Product product)
    {
        if (!ModelState.IsValid) return View(product);

        var user = await _userManager.GetUserAsync(HttpContext.User);
        await _productRepository.CreateProductAsync(product);
        await _productRepository.CreateAudit(product, null, "Create", user);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        if (id == null) return NotFound();

        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null) return NotFound();

        return View(product);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Quantity,Price")] Product product)
    {
        if (id != product.Id) return NotFound();

        if (!ModelState.IsValid) return View(product);

        try
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var oldProduct = await _productRepository.GetOldValueAsync(id);
            var newProduct = await _productRepository.UpdateProductAsync(product);
            await _productRepository.CreateAudit(newProduct, oldProduct, "Edit", user);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_productRepository.GetProductByIdAsync(product.Id) == null)
                return NotFound();
            else
                throw;
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        if (id == null) return NotFound();

        var product = await _productRepository.GetProductByIdAsync(id);

        if (product == null) return NotFound();

        return View(product);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _productRepository.DeleteProductAsync(id);
        if (product == null) return NotFound();

        var user = await _userManager.GetUserAsync(HttpContext.User);
        await _productRepository.CreateAudit(product, null, "Delete", user);

        return RedirectToAction(nameof(Index));
    }
}
