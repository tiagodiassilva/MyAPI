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

        [HttpGet("usd-eur")]
        public async Task<IActionResult> GetUsdEur()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://economia.awesomeapi.com.br/last/USD-EUR");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var currencyData = JsonSerializer.Deserialize<UsdEurResponse>(content);
                    return Ok(currencyData?.UsdEur);
                }
                
                return StatusCode((int)response.StatusCode, "Failed to fetch USD-EUR data");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching USD-EUR data");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("nyse")]
        public async Task<IActionResult> GetNyse()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                // Using Finnhub API with demo token for SPY (S&P 500 ETF)
                var response = await client.GetAsync("https://finnhub.io/api/v1/quote?symbol=SPY&token=demo");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var nyseData = JsonSerializer.Deserialize<NyseQuote>(content);
                    return Ok(nyseData);
                }

                return StatusCode((int)response.StatusCode, "Failed to fetch NYSE data");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching NYSE data");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // Fetch all data sources concurrently
                var currencyTask = client.GetAsync("https://economia.awesomeapi.com.br/last/USD-BRL");
                var stockTask = client.GetAsync("https://api.hgbrasil.com/finance");
                var usdEurTask = client.GetAsync("https://economia.awesomeapi.com.br/last/USD-EUR");
                var nyseTask = client.GetAsync("https://finnhub.io/api/v1/quote?symbol=SPY&token=demo");

                await Task.WhenAll(currencyTask, stockTask, usdEurTask, nyseTask);

                var currencyResponse = await currencyTask;
                var stockResponse = await stockTask;
                var usdEurResponse = await usdEurTask;
                var nyseResponse = await nyseTask;

                object? currencyResult = null;
                object? stockResult = null;
                object? usdEurResult = null;
                object? nyseResult = null;

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

                if (usdEurResponse.IsSuccessStatusCode)
                {
                    var content = await usdEurResponse.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<UsdEurResponse>(content);
                    usdEurResult = data?.UsdEur;
                }

                if (nyseResponse.IsSuccessStatusCode)
                {
                    var content = await nyseResponse.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<NyseQuote>(content);
                    nyseResult = data;
                }

                return Ok(new
                {
                    UsdBrl = currencyResult,
                    Ibovespa = stockResult,
                    UsdEur = usdEurResult,
                    Nyse = nyseResult,
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
