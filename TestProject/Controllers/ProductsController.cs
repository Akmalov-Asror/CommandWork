using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.Domains;
using TestProject.ExtensionFunctions;
using TestProject.Services.Interfaces;
using TestProject.ViewModels;

namespace TestProject.Controllers;
public class ProductsController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly UserManager<User> _userManager;
    private readonly VatCalculator _vatCalculator;

    public ProductsController(IProductRepository productRepository, UserManager<User> userManager, VatCalculator vatCalculator)
    {
        _productRepository = productRepository;
        _userManager = userManager;
        _vatCalculator = vatCalculator;
    }

    [Authorize(Roles = "ADMIN, USER")]
    public async Task<IActionResult> Index()
    {
        var products = await _productRepository.GetAllProducts();

        var productViewModels = products.Select(p => new ProductViewModel
        {
            Id = p.Id,
            Title = p.Title,
            Quantity = p.Quantity,
            Price = p.Price,
            TotalPriceWithVAT = _vatCalculator.CalculateTotalPriceWithVat(p.Quantity, p.Price),
        }).ToList();

        return View(productViewModels);
    }
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
           if (id == null) return NotFound();

                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null) return NotFound();

                return View(product);
        }
        catch (Exception ex)
        {
            return RedirectToAction("Index", "NotFoundPage");
        }
     
    }
    [Authorize(Roles = "ADMIN")]
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
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            if (id == null) return NotFound();

            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return View(product);
        } catch(Exception ex)
        {
          return  RedirectToAction("Index", "NotFoundPage");
        }
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


    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (id == null) return NotFound();

            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null) return NotFound();

            return View(product);
        }
        catch
        {
            return RedirectToAction("Index", "NotFoundPage");
        }
    
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
