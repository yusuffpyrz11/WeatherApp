using WeatherApp.Models;

namespace WeatherApp.Interfaces
{
    /// <summary>
    /// Hava durumu servisinin sözleşmesini (contract) tanımlayan arayüz.
    /// 
    /// SOLID - Dependency Inversion Principle (DIP):
    /// Yüksek seviyeli modüller (Program.cs) bu arayüze bağımlıdır,
    /// somut implementasyona (WeatherService) değil. Bu sayede test sırasında
    /// mock servis kolayca enjekte edilebilir.
    /// </summary>
    public interface IWeatherService
    {
        /// <summary>
        /// Belirtilen şehir için güncel hava durumu bilgilerini getirir.
        /// </summary>
        /// <param name="cityName">Sorgulanacak şehir adı (örn. "Istanbul")</param>
        /// <returns>Başarı veya hata bilgisi içeren WeatherResult</returns>
        Task<WeatherResult<WeatherResponse>> GetWeatherAsync(string cityName);
    }
}
