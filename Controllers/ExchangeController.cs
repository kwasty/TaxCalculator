using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using System.Net;
using TaxCalculator.Models;
using TaxCalculator.Services;

namespace TaxCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private static readonly IEnumerable<string> SupportedCurrencies = new List<string> { "CAD", "USD", "EUR" };

        private readonly ILogger<ExchangeController> _logger;
        private readonly IFixerService _fixerService;

        public ExchangeController(ILogger<ExchangeController> logger, IFixerService fixerService)
        {
            _logger = logger;
            _fixerService = fixerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string invoiceDate, decimal preTaxAmount, string paymentCurrency)
        {
            try
            {
                paymentCurrency = paymentCurrency.ToUpper();
                if (!SupportedCurrencies.Contains(paymentCurrency))
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, "Only CAD and USD are supported as currencies.");
                }

                DateOnly date;
                var success = DateOnly.TryParse(invoiceDate, out date);

                if (!success)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, "Badly formatted date.");
                }

                ConvertedCurrency convertedCurrencty = await _fixerService.ConvertFromEUR(preTaxAmount, paymentCurrency, date);

                return Ok(convertedCurrencty);
            }
            catch (WebException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error communicating with the Fixer API.");
            } catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Something went wrong");
            }
        }
    }
}
