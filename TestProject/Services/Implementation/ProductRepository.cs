using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TestProject.Data;
using TestProject.Domains;
using TestProject.Services.Interfaces;

namespace TestProject.Services.Implementation;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _appDbContext;
    public ProductRepository(AppDbContext appDbContext) => _appDbContext = appDbContext;
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await _appDbContext.Products
            .OrderBy(p => p.Id)
            .ToListAsync();
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        await _appDbContext.Products.AddAsync(product);
        await _appDbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        var currentProduct = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        return currentProduct ?? throw new Exception("Product not found");
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        var currentProduct = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == product.Id);
        if (currentProduct == null) throw new Exception("Product not found");

        currentProduct.Price = product.Price;
        currentProduct.Quantity = product.Quantity;
        currentProduct.Title = product.Title;

        await _appDbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product> DeleteProductAsync(int id)
    {
        var currentProduct = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (currentProduct == null) throw new Exception("Product not found");

        _appDbContext.Products.Remove(currentProduct);
        await _appDbContext.SaveChangesAsync();
        return currentProduct;
    }

    public async Task<Product> GetOldValueAsync(int id) => await _appDbContext.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Product> CreateAudit(Product newProduct, Product oldProduct, string actionType, User user)
    {
        var auditTrailRecord = new AuditLog
        {
            UserName = user.UserName,
            Action = actionType,
            ControllerName = "Product",
            DateTime = DateTime.UtcNow,
            OldValue = JsonConvert.SerializeObject(oldProduct, Formatting.Indented),
            NewValue = JsonConvert.SerializeObject(newProduct, Formatting.Indented)
        };

        _appDbContext.AuditLog.Add(auditTrailRecord);
        try
        {
            await _appDbContext.SaveChangesAsync();
            return newProduct;
        }
        catch (Exception ex)
        {
            throw new Exception("Error saving audit log.", ex);
        }
    }
}
