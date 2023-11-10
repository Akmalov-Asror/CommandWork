using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.Data;
using TestProject.Domains;

namespace TestProject.Controllers;
[Authorize]
public class ProductsController : Controller
{
    private readonly AppDbContext _context;
    public ProductsController(AppDbContext context) => _context = context;
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Index() => _context.Products != null ? View(await _context.Products.ToListAsync()) : Problem("Entity set 'AppDbContext.Products'  is null.");

    public async Task<IActionResult> UserView()
    {
        var listProduct = await _context.Products.ToListAsync();
        return View("UserView", listProduct);
    }
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Products == null) return NotFound();

        var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
        if (product == null) return NotFound();

        return View(product);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Quantity,Price")] Product product)
    {
        if (!ModelState.IsValid) return View(product);
        _context.Add(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Products == null) return NotFound();

        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Quantity,Price")] Product product)
    {
        if (id != product.Id) return NotFound();

        if (!ModelState.IsValid) return View(product);
        try
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(product.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return RedirectToAction("Index","Home");
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Products == null) return NotFound();

        var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

        if (product == null) return NotFound();

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Products == null) return Problem("Entity set 'AppDbContext.Products'  is null.");
        var product = await _context.Products.FindAsync(id);
        if (product != null) _context.Products.Remove(product);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id) => (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
}