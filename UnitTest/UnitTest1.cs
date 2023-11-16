using TestProject.Domains;

namespace UnitTest;

public class UnitTest1
{
    // Assume ProductService handles product-related operations
    public class ProductService
    {
        private readonly double VAT;

        public ProductService(double vat)
        {
            VAT = vat;
        }

        public double CalculateTotalPriceWithVAT(Product product)
        {
            return (product.Quantity * product.Price) * (1 + VAT);
        }
    }

    // Assume AuthService handles authentication
    public class AuthService
    {
        public bool ValidateUser(string username, string password)
        {
            // Your authentication logic here (compare with stored usernames and passwords)
            // For simplicity, let's assume there are two predefined users: admin and user
            return (username == "admin" && password == "Admin*123") ||
                   (username == "user" && password == "Admin*123");
        }
    }

    // Assume Product model is defined as in the previous examples

    // Unit tests using xUnit and Moq for mocking
    public class ProductServiceTests
    {
        [Fact]
        public void CalculateTotalPriceWithVAT_ShouldCalculateCorrectly()
        {
            // Arrange
            var productService = new ProductService(0.2); // Assuming 20% VAT
            var product = new Product { Quantity = 5, Price = 10 };

            // Act
            var result = productService.CalculateTotalPriceWithVAT(product);

            // Assert
            Assert.Equal(60, result); // Expected result with 20% VAT: (5 * 10) * (1 + 0.2) = 60
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