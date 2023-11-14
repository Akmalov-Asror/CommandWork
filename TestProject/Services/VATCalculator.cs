using Microsoft.Extensions.Configuration;

namespace TestProject.Services
{
    public class VATCalculator
    {
        private decimal VAT;

        public VATCalculator(IConfiguration configuration)
        {
            // Retrieve VAT percentage from appSettings
            VAT = configuration.GetValue<decimal>("VATSettings:VATPercentage");
        }

        public decimal CalculateTotalPriceWithVAT(int quantity, decimal price)
        {
            return (quantity * price) * (1 + VAT);
        }
    }
}
