using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases
{

    public class ExchangeRateService : IExchangeRateService
    {
        private readonly HttpClient _httpClient;

        // Fixer.io Free plan API key
        private readonly string _apiKey = "ff31d9ef14c99bf6056a3131f901c2c2";

        // Keširanje kurseva: key = "FROM_TO", value = (rate, expiry)
        private static readonly Dictionary<string, (decimal Rate, DateTime Expiry)> _cache = new();
        private static readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

        // Lista valuta
        private static readonly Dictionary<string, string> _currencies = new()
        {
            {"EUR", "Euro"},
            {"USD", "United States Dollar"},
            {"RSD", "Serbian Dinar"},
            {"GBP", "British Pound"},
            {"CHF", "Swiss Franc"},
            {"JPY", "Japanese Yen"},
            {"CAD", "Canadian Dollar"},
            {"AUD", "Australian Dollar"}
        };

        public ExchangeRateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<ExchangeRateDto>> ConvertCurrencyAsync(decimal amount, string fromCurrency, string toCurrency)
        {
            try
            {
                string key = $"{fromCurrency}_{toCurrency}";

                // Provera keša
                if (_cache.ContainsKey(key) && _cache[key].Expiry > DateTime.UtcNow)
                {
                    var cachedRate = _cache[key].Rate;
                    return Result.Ok(new ExchangeRateDto
                    {
                        FromCurrency = fromCurrency,
                        ToCurrency = toCurrency,
                        Amount = amount,
                        ConvertedAmount = amount * cachedRate,
                        Rate = cachedRate,
                        Date = DateTime.UtcNow.ToString("yyyy-MM-dd")
                    });
                }

                // Free plan: EUR je baza
                string symbols = string.Join(",", new[] { fromCurrency, toCurrency });
                string url = $"https://data.fixer.io/api/latest?access_key={_apiKey}&symbols={symbols}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonResponse);

                if (!doc.RootElement.TryGetProperty("success", out var successProp) || !successProp.GetBoolean())
                    return Result.Fail("API returned unsuccessful response.");

                var rates = doc.RootElement.GetProperty("rates");
                decimal rateFrom = rates.GetProperty(fromCurrency).GetDecimal();
                decimal rateTo = rates.GetProperty(toCurrency).GetDecimal();

                // Konverzija iz fromCurrency u toCurrency
                decimal convertedAmount = amount / rateFrom * rateTo;
                decimal rate = rateTo / rateFrom;
                string date = doc.RootElement.GetProperty("date").GetString();

                // Čuvanje u keš
                _cache[key] = (rate, DateTime.UtcNow + _cacheDuration);

                return Result.Ok(new ExchangeRateDto
                {
                    FromCurrency = fromCurrency,
                    ToCurrency = toCurrency,
                    Amount = amount,
                    ConvertedAmount = convertedAmount,
                    Rate = rate,
                    Date = date
                });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Greška prilikom konverzije: {ex.Message}");
            }
        }

        public Task<Result<Dictionary<string, string>>> GetAllCurrenciesAsync()
        {
            return Task.FromResult(Result.Ok(_currencies));
        }
    }
}

