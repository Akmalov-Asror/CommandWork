using System;
using TestProject.Services.Interface;
using TestProject.Domains;
using TestProject.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace TestProject.Services.Repository

{
    public class ProductRepository : IProductRepository
	{
        private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await  _appDbContext.Products.ToListAsync();
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
           await _appDbContext.Products.AddAsync(product);

            _appDbContext.SaveChanges();
            return product;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {

            var currentProduct = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (currentProduct == null)
            {
                throw new Exception("Product not found");
            }
            return currentProduct;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
           _appDbContext.Products.Update(product);
            _appDbContext.SaveChanges();
            return product;

        }
        public async Task<Product> DeleteProductAsync(int id)
        {
            var currentProduct = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (currentProduct == null)
            {
                throw new Exception("Product not found");
            }

            _appDbContext.Products.Remove(currentProduct);
            await _appDbContext.SaveChangesAsync();
            return currentProduct;
            
        }
        public async Task<Product> GetOldValueAsync(int id)
        {
            return await _appDbContext.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product> CreateAudit(Product entity,Product oldValue, string actionType)
        {
            //var userId = await GetCurrentUserAsync();

            var auditTrailRecord = new AuditLog();
            //auditTrailRecord.UserName = user.UserName;
            auditTrailRecord.Action = actionType;
            auditTrailRecord.ControllerName = "Product";
            if (actionType == "Delete")
            {
                auditTrailRecord.OldValue = JsonConvert.SerializeObject(entity, Formatting.Indented);
            }
            else if (actionType == "Edit")
            {
                auditTrailRecord.OldValue = JsonConvert.SerializeObject(oldValue, Formatting.Indented);
                auditTrailRecord.NewValue = JsonConvert.SerializeObject(entity, Formatting.Indented);
            }
            else {
            }
            auditTrailRecord.DateTime = DateTime.UtcNow;

            _appDbContext.AuditLog.Add(auditTrailRecord);
            try
            {
                await _appDbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                throw;
            }
        }

    }

   
}