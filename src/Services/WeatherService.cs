using System.Net;
using System.Text.Json;
using WeatherApp.Configuration;
using WeatherApp.Interfaces;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    /// <summary>
    /// OpenWeatherMap API ile iletişim kuran hava durumu servis implementasyonu.
    /// 
    /// SOLID - Single Responsibility Principle (SRP):
    /// Bu sınıf yalnızca API'den hava durumu verisi çekmekten sorumludur.
    /// JSON parse, ekran gösterimi veya geçmiş kaydı bu sınıfın işi değildir.
    /// </summary>
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        // JsonSerializerOptions nesnesini her çağrıda yeniden oluşturmamak için static tanımlandı
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Constructor Injection (DI Pattern):
        /// HttpClient dışarıdan enjekte edilir — bu test yazımını kolaylaştırır
        /// ve HttpClient'ın yaşam döngüsü IHttpClientFactory tarafından yönetilir.
        /// </summary>
        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.Timeout = TimeSpan.FromSeconds(AppSettings.HttpTimeoutSeconds);
        }

        /// <inheritdoc />
        public async Task<WeatherResult<WeatherResponse>> GetWeatherAsync(string cityName)
        {
            // ── Girdi Doğrulama ───────────────────────────────────────────────
            if (string.IsNullOrWhiteSpace(cityName))
                return WeatherResult<WeatherResponse>.Failure("Şehir adı boş olamaz.");

            // ── URL Oluşturma ─────────────────────────────────────────────────
            // Uri.EscapeDataString → "New York" → "New%20York" (güvenli URL encode)
            var encodedCity = Uri.EscapeDataString(cityName.Trim());
            var url = string.Format(AppSettings.BaseApiUrl, encodedCity, AppSettings.ApiKey, AppSettings.Units);

            try
            {
                // ── HTTP İsteği ───────────────────────────────────────────────
                var response = await _httpClient.GetAsync(url);

                // ── Hata Durumu Yönetimi ─────────────────────────────────────
                if (!response.IsSuccessStatusCode)
                {
                    return response.StatusCode switch
                    {
                        HttpStatusCode.NotFound =>
                            WeatherResult<WeatherResponse>.Failure(
                                $"'{cityName}' şehri bulunamadı. Lütfen şehir adını kontrol edin."),

                        HttpStatusCode.Unauthorized =>
                            WeatherResult<WeatherResponse>.Failure(
                                "API anahtarı geçersiz veya eksik. AppSettings.cs dosyasını kontrol edin."),

                        HttpStatusCode.TooManyRequests =>
                            WeatherResult<WeatherResponse>.Failure(
                                "API istek limiti aşıldı. Lütfen birkaç dakika bekleyin."),

                        _ => WeatherResult<WeatherResponse>.Failure(
                                $"API hatası: {(int)response.StatusCode} - {response.ReasonPhrase}")
                    };
                }

                // ── JSON Deserialize ──────────────────────────────────────────
                var jsonContent = await response.Content.ReadAsStringAsync();

                var weatherData = JsonSerializer.Deserialize<WeatherResponse>(jsonContent, _jsonOptions);

                if (weatherData is null)
                    return WeatherResult<WeatherResponse>.Failure("API'den geçersiz veri döndü.");

                return WeatherResult<WeatherResponse>.Success(weatherData);
            }
            catch (HttpRequestException ex)
            {
                return WeatherResult<WeatherResponse>.Failure(
                    $"Ağ bağlantısı hatası: {ex.Message}\nİnternet bağlantınızı kontrol edin.");
            }
            catch (TaskCanceledException)
            {
                return WeatherResult<WeatherResponse>.Failure(
                    $"İstek zaman aşımına uğradı ({AppSettings.HttpTimeoutSeconds} saniye). Lütfen tekrar deneyin.");
            }
            catch (JsonException ex)
            {
                return WeatherResult<WeatherResponse>.Failure(
                    $"Veri ayrıştırma hatası: {ex.Message}");
            }
        }
    }
}
