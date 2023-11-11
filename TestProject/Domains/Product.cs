namespace TestProject.Domains;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; }    
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    // VAT value retrieved from a configuration file
    private static decimal VAT;// Adjust this value accordingly
    // Total Price with VAT property
    public decimal TotalPriceWithVAT => (Quantity * Price) * (1 + VAT);

    public static void SetVAT(decimal vat)
    {
        VAT = vat;
    }
}


public class VATSettingsModel
{
    public decimal VATPercentage { get; set; }
}