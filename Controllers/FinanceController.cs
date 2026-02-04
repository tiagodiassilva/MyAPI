using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using System.Text.Json;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinanceController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<FinanceController> _logger;

        public FinanceController(IHttpClientFactory httpClientFactory, ILogger<FinanceController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet("currency")]
        public async Task<IActionResult> GetCurrency()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://economia.awesomeapi.com.br/last/USD-BRL");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var currencyData = JsonSerializer.Deserialize<CurrencyResponse>(content);
                    return Ok(currencyData?.UsdBrl);
                }
                
                return StatusCode((int)response.StatusCode, "Failed to fetch currency data");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching currency data");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stock")]
        public async Task<IActionResult> GetStock()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                // Using HG Brasil Finance API (Free tier/Public)
                var response = await client.GetAsync("https://api.hgbrasil.com/finance");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var financeData = JsonSerializer.Deserialize<HgFinanceResponse>(content);
                    return Ok(financeData?.Results?.Stocks?.Ibovespa);
                }

                return StatusCode((int)response.StatusCode, "Failed to fetch stock data");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching stock data");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // Fetch concurrently
                var currencyTask = client.GetAsync("https://economia.awesomeapi.com.br/last/USD-BRL");
                var stockTask = client.GetAsync("https://api.hgbrasil.com/finance");

                await Task.WhenAll(currencyTask, stockTask);

                var currencyResponse = await currencyTask;
                var stockResponse = await stockTask;

                object? currencyResult = null;
                object? stockResult = null;

                if (currencyResponse.IsSuccessStatusCode)
                {
                    var content = await currencyResponse.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<CurrencyResponse>(content);
                    currencyResult = data?.UsdBrl;
                }

                if (stockResponse.IsSuccessStatusCode)
                {
                    var content = await stockResponse.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<HgFinanceResponse>(content);
                    stockResult = data?.Results?.Stocks?.Ibovespa;
                }

                return Ok(new
                {
                    Currency = currencyResult,
                    Stock = stockResult,
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching summary data");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
