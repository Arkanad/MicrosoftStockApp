using StockAppWithConfiguration.ServiceContracts;
using System.Text.Json;

namespace StockAppWithConfiguration.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public FinnhubService (IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
           using (HttpClient httpClient = _httpClientFactory.CreateClient()){

                

                HttpRequestMessage httpRequest = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
                };

                HttpResponseMessage responseMessage = await httpClient.SendAsync(httpRequest);

                Stream streamMessage = responseMessage.Content.ReadAsStream();

                StreamReader streamReader = new StreamReader(streamMessage);

                string response = streamReader.ReadToEnd();

                Dictionary<string, object> responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                if (responseDictionary == null)
                {
                    throw new InvalidOperationException("no response from finnub server");
                }

                if (responseDictionary.ContainsKey("error"))
                {
                    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
                }

                return responseDictionary;
            }
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
                };

                HttpResponseMessage responseMessage = await httpClient.SendAsync(httpRequest);

                Stream streamMessage = responseMessage.Content.ReadAsStream();

                StreamReader streamReader = new StreamReader(streamMessage);

                string response = streamReader.ReadToEnd();

                Dictionary<string, object> responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                if (responseDictionary == null)
                {
                    throw new InvalidOperationException("no response from finnub server");
                }

                if (responseDictionary.ContainsKey("error"))
                {
                    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
                }

                return responseDictionary;
            }
        }
    }
}
