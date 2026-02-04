using System.Text.Json.Serialization;

namespace MyApp.Models
{
    public class CurrencyResponse
    {
        [JsonPropertyName("USDBRL")]
        public CurrencyInfo? UsdBrl { get; set; }
    }

    public class CurrencyInfo
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }
        
        [JsonPropertyName("codein")]
        public string? CodeIn { get; set; }
        
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        
        [JsonPropertyName("bid")]
        public string? Bid { get; set; }

        [JsonPropertyName("ask")]
        public string? Ask { get; set; }

        [JsonPropertyName("create_date")]
        public string? CreateDate { get; set; }
    }

    public class HgFinanceResponse
    {
        [JsonPropertyName("results")]
        public HgResults? Results { get; set; }
    }

    public class HgResults
    {
        [JsonPropertyName("stocks")]
        public HgStocks? Stocks { get; set; }
    }

    public class HgStocks
    {
        [JsonPropertyName("IBOVESPA")]
        public StockInfo? Ibovespa { get; set; }
    }

    public class StockInfo
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("points")]
        public double Points { get; set; }

        [JsonPropertyName("variation")]
        public double Variation { get; set; }
    }

    // USD-EUR Currency Models
    public class UsdEurResponse
    {
        [JsonPropertyName("USDEUR")]
        public CurrencyInfo? UsdEur { get; set; }
    }

    // NYSE Stock Models (Finnhub API)
    public class NyseQuote
    {
        [JsonPropertyName("c")]
        public double CurrentPrice { get; set; }

        [JsonPropertyName("d")]
        public double Change { get; set; }

        [JsonPropertyName("dp")]
        public double PercentChange { get; set; }

        [JsonPropertyName("h")]
        public double HighPrice { get; set; }

        [JsonPropertyName("l")]
        public double LowPrice { get; set; }

        [JsonPropertyName("o")]
        public double OpenPrice { get; set; }

        [JsonPropertyName("pc")]
        public double PreviousClose { get; set; }

        [JsonPropertyName("t")]
        public long Timestamp { get; set; }
    }
}
