using System.Net;
using System.Text.Json;
using TaxCalculator.Models;

namespace TaxCalculator.Services
{
    public class FixerService : IFixerService
    {
        private static readonly IDictionary<string, decimal> TaxRates = new Dictionary<string, decimal> {
            { "CAD", 0.11m },
            { "USD", 0.10m },
            { "EUR", 0.09m }
        };

        private readonly IConfiguration _configuration;

        public FixerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ConvertedCurrency> ConvertFromEUR(decimal amount, string currency, DateOnly date)
        {
            var rate = await GetRate("EUR", currency, date);
            return GetConvertedCurrency(amount, currency, rate);
        }

        private ConvertedCurrency GetConvertedCurrency(decimal amount, string currency, decimal? rate)
        {
            if (!rate.HasValue)
            {
                return new ConvertedCurrency();
            }

            var preTaxTotal = Math.Round(amount * rate.Value, 2);
            var taxRate = TaxRates[currency];
            var taxAmount = Math.Round(preTaxTotal * taxRate, 2);
            var grandTotal = preTaxTotal + taxAmount;

            var convertedCurrencty = new ConvertedCurrency
            {
                ExchangeRate = rate.Value,
                GrandTotal = grandTotal,
                PreTaxTotal = preTaxTotal,
                TaxAmount = taxAmount
            };
            return convertedCurrencty;
        }

        private async Task<decimal?> GetRate(string from, string to, DateOnly date)
        {
            var apiKey = GetApiKey();
            var baseUrl = GetFixerBaseURL();
            var formattedDate = date.ToString("yyyy-MM-dd");
            var url = $"{baseUrl}/{formattedDate}?access_key={apiKey}&base={from}&symbols={to}";

            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(url);
                var stringResult = result.Content.ReadAsStream();

                var json = await JsonSerializer.DeserializeAsync<FixerResponse>(stringResult);
                if (json == null)
                {
                    throw new WebException("No response recieved from Fixer.");
                }

                if (json.Success == true)
                {
                    if (json.Rates == null || json.Rates.Count == 0)
                    {
                        return null;
                    }
                    return json.Rates[to];
                } else
                {
                    if (json.Error == null)
                    {
                        throw new WebException("Unknown Fixer error.");
                    }
                    throw new WebException(json.Error["info"].ToString());
                }
            }
        }

        private string GetFixerBaseURL()
        {
            var baseUrlConfig = _configuration.GetSection("Settings").GetSection("FixerBaseURL");

            if (baseUrlConfig == null)
            {
                throw new Exception("Fixer URL is missing");
            }

            return baseUrlConfig.Value.ToString();
        }

        private string GetApiKey()
        {
            var apiKeyConfig = _configuration.GetSection("Settings").GetSection("FixerAPIKey");

            if (apiKeyConfig == null)
            {
                throw new Exception("Api Key is Missing");
            }

            return apiKeyConfig.Value.ToString();
        }
    }
}
