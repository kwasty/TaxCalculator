namespace TaxCalculator.Models
{
    public class ConvertedCurrency
    {
        public decimal PreTaxTotal  { get; set; }
        public decimal TaxAmount    { get; set; }
        public decimal GrandTotal   { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}
