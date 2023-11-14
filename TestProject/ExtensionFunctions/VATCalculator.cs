using Microsoft.Extensions.Configuration;

namespace TestProject.ExtensionFunctions;

public class VatCalculator
{
    private decimal VAT;
    public VatCalculator(IConfiguration configuration) => VAT = configuration.GetValue<decimal>("VATSettings:VATPercentage");
    public decimal CalculateTotalPriceWithVat(int quantity, decimal price) => quantity * price * (1 + VAT);
}