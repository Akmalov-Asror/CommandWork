using TestProject.Domains;

namespace TestProject.Services.Interfaces;

public interface IProductRepository
{
    /// <summary>
    /// Retrieves a product by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the product to retrieve.</param>
    /// <returns>
    ///     Returns a Task containing the Product corresponding to the provided identifier.
    /// </returns>
    public Task<Product> GetProductByIdAsync(int id);

    /// <summary>
    /// Retrieves all products asynchronously.
    /// </summary>
    /// <returns>
    ///     Returns a Task containing an IEnumerable of all available Product items.
    /// </returns>
    public Task<IEnumerable<Product>> GetAllProducts();

    /// <summary>
    /// Creates a new product asynchronously.
    /// </summary>
    /// <param name="entity">Product entity representing the new product to be created.</param>
    /// <returns>
    ///     Returns a Task containing the created Product entity.
    /// </returns>
    public Task<Product> CreateProductAsync(Product entity);

    /// <summary>
    /// Updates an existing product asynchronously.
    /// </summary>
    /// <param name="entity">Product entity representing the updated product information.</param>
    /// <returns>
    ///     Returns a Task containing the updated Product entity.
    /// </returns>
    public Task<Product> UpdateProductAsync(Product entity);

    /// <summary>
    /// Deletes a product by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the product to delete.</param>
    /// <returns>
    ///     Returns a Task containing the deleted Product entity.
    /// </returns>
    public Task<Product> DeleteProductAsync(int Id);

    /// <summary>
    /// Retrieves the old value of a product by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the product to retrieve the old value.</param>
    /// <returns>
    ///     Returns a Task containing the old Product entity.
    /// </returns>
    public Task<Product> GetOldValueAsync(int id);

    /// <summary>
    /// Creates an audit log entry for a product operation asynchronously.
    /// </summary>
    /// <param name="entity">Product entity representing the current state of the product.</param>
    /// <param name="oldValue">Product entity representing the previous state of the product.</param>
    /// <param name="actionType">The type of action performed (e.g., 'Create', 'Update', 'Delete').</param>
    /// <param name="user">User object representing the user performing the action.</param>
    /// <returns>
    ///     Returns a Task representing the completion of the audit log creation.
    /// </returns>
    public Task<Product> CreateAudit(Product entity, Product oldValue, string actionType, User user);

}