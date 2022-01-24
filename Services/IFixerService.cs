using TaxCalculator.Models;

namespace TaxCalculator.Services
{
    public interface IFixerService
    {
        public Task<ConvertedCurrency> ConvertFromEUR(decimal amount, string currency, DateOnly date);
    }
}
