using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using TaxCalculator.Services;
using Xunit;

namespace Tests
{
    public class FixerServiceTests
    {
        [Fact]
        public async Task TestUSD()
        {
            var config = MockConfiguration();
            IFixerService fixerService = new FixerService(config);

            var convert = await fixerService.ConvertFromEUR(123.45m, "USD", DateOnly.Parse("2020-08-05"));

            Assert.NotNull(convert);
            Assert.Equal(146.57m, convert.PreTaxTotal);
            Assert.Equal(14.66m, convert.TaxAmount);
            Assert.Equal(161.23m, convert.GrandTotal);
            Assert.Equal(1.187247m, convert.ExchangeRate);
        }

        [Fact]
        public async Task TestEUR()
        {
            var config = MockConfiguration();
            IFixerService fixerService = new FixerService(config);

            var convert = await fixerService.ConvertFromEUR(1000.00m, "EUR", DateOnly.Parse("2019-07-12"));

            Assert.NotNull(convert);
            Assert.Equal(1000.00m, convert.PreTaxTotal);
            Assert.Equal(90.00m, convert.TaxAmount);
            Assert.Equal(1090.00m, convert.GrandTotal);
            Assert.Equal(1m, convert.ExchangeRate);
        }

        [Fact]
        public async Task TestCAD()
        {
            var config = MockConfiguration();
            IFixerService fixerService = new FixerService(config);

            var convert = await fixerService.ConvertFromEUR(6543.21m, "CAD", DateOnly.Parse("2020-08-19"));

            Assert.NotNull(convert);
            Assert.Equal(10239.07m, convert.PreTaxTotal);
            Assert.Equal(1126.30m, convert.TaxAmount);
            Assert.Equal(11365.37m, convert.GrandTotal);
            Assert.Equal(1.564839m, convert.ExchangeRate);
        }

        [Fact]
        public async Task TestError()
        {
            var config = MockConfiguration();
            IFixerService fixerService = new FixerService(config);

            try
            {
                var convert = await fixerService.ConvertFromEUR(6543.21m, "MadeUpCurrency", DateOnly.Parse("2020-08-19"));
            }
            catch (Exception e)
            {
                Assert.Equal("You have provided one or more invalid Currency Codes. [Required format: currencies=EUR,USD,GBP,...]", e.Message);
            }
        }

        private IConfiguration MockConfiguration()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            return config;
        }
    }
}