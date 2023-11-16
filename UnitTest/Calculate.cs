using TestProject.Domains;

namespace UnitTest;

public class Calculate
{
    public class ProductService
    {
        private readonly double VAT;
        /// <summary>
        /// Initializes a new instance of the ProductService class with a specified VAT rate.
        /// </summary>
        /// <param name="vat">The value-added tax (VAT) rate.</param>
        public ProductService(double vat) => VAT = vat;
        /// <summary>
        /// Calculates the total price of a product including VAT.
        /// </summary>
        /// <param name="product">The product for which the total price is calculated.</param>
        /// <returns>The total price including VAT.</returns>
        public double CalculateTotalPriceWithVat(Product product) => (product.Quantity * product.Price) * (1 + VAT);
    }
    /// <summary>
    /// Service responsible for user authentication.
    /// </summary>
    public class AuthService
    {
        /// <summary>
        /// Validates a user's credentials.
        /// </summary>
        /// <param name="username">The username to validate.</param>
        /// <param name="password">The password to validate.</param>
        /// <returns>True if the credentials are valid; otherwise, false.</returns>
        public bool ValidateUser(string username, string password)
        {
            return (username == "admin" && password == "Admin*123") ||
                   (username == "user" && password == "Admin*123");
        }
    }

    public class ProductServiceTests
    {
        [Fact]
        public void CalculateTotalPriceWithVAT_ShouldCalculateCorrectly()
        {
            // Arrange
            var productService = new ProductService(0.2); // Assuming 20% VAT
            var product = new Product { Quantity = 5, Price = 10 };

            // Act
            var result = productService.CalculateTotalPriceWithVat(product);

            // Assert
            Assert.Equal(60, result);
        }
    }

    public class AuthServiceTests
    {
        [Fact]
        public void ValidateUser_ShouldReturnTrueForValidCredentials()
        {
            // Arrange
            var authService = new AuthService();

            // Act
            var result = authService.ValidateUser("admin", "Admin*123");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateUser_ShouldReturnFalseForInvalidCredentials()
        {
            // Arrange
            var authService = new AuthService();

            // Act
            var result = authService.ValidateUser("invaliduser", "invalidpass");

            // Assert
            Assert.False(result);
        }
    }

}