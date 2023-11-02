using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockAppWithConfiguration.Models;
using StockAppWithConfiguration.Services;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace StockAppWithConfiguration.Controllers
{
    public class TradeController : Controller
    {
        private readonly FinnhubService _finnhubService;
        private readonly IConfiguration _configuration;
        private readonly IOptions<TradingOptions> _tradingOptions;

        public TradeController(FinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, IConfiguration configuration)
        {
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions;
            _configuration = configuration;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            if (_tradingOptions.Value.DefaultStockSymbol == null)
            {
                _tradingOptions.Value.DefaultStockSymbol = "MSFT";
                
            }
            
            Dictionary<string, object>? companyProfileDictionary = await _finnhubService.GetCompanyProfile(_tradingOptions.Value.DefaultStockSymbol);

            Dictionary<string, object>? StockDictionary = await _finnhubService.GetStockPriceQuote(_tradingOptions.Value.DefaultStockSymbol);

            StockTrade stock = new StockTrade()
            {
                StockSymbol = _tradingOptions.Value.DefaultStockSymbol
            };

            if(companyProfileDictionary != null && StockDictionary != null)
            {
                stock = new StockTrade()
                {
                    StockSymbol = _tradingOptions.Value.DefaultStockSymbol,
                    Price = Convert.ToDouble(StockDictionary["c"].ToString(), CultureInfo.InvariantCulture),
                    StockName = companyProfileDictionary["name"].ToString()
                };
            }
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];
            return View(stock);
        }

        //CompanyProfile companyProfile = new CompanyProfile()
        //{
        //    Country = finnhubResponse["country"].ToString(),
        //    Exchange = finnhubResponse["exchange"].ToString(),
        //    Ipo = finnhubResponse["ipo"].ToString(),
        //    Currency = finnhubResponse["currency"].ToString(),
        //    Name = finnhubResponse["name"].ToString(),
        //    Logo = finnhubResponse["logo"].ToString(),
        //    Ticker = finnhubResponse["ticker"].ToString(),
        //    Weburl = finnhubResponse["weburl"].ToString()
        //};
    }
}
