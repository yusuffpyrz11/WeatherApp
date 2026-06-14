using System.Text.Json.Serialization;

namespace WeatherApp.Models
{
    /// <summary>
    /// OpenWeatherMap API'sinden dönen ana yanıt modeli.
    /// API dökümantasyonu: https://openweathermap.org/current
    /// </summary>
    public class WeatherResponse
    {
        /// <summary>Şehrin adı (örn. "Istanbul")</summary>
        [JsonPropertyName("name")]
        public string CityName { get; set; } = string.Empty;

        /// <summary>Sıcaklık, nem gibi ölçüm verilerini içerir</summary>
        [JsonPropertyName("main")]
        public MainData Main { get; set; } = new();

        /// <summary>Rüzgar bilgilerini içerir</summary>
        [JsonPropertyName("wind")]
        public WindData Wind { get; set; } = new();

        /// <summary>Hava durumu açıklamalarının listesi</summary>
        [JsonPropertyName("weather")]
        public List<WeatherDescription> Weather { get; set; } = new();

        /// <summary>Ülke kodu ve gün doğumu/batımı bilgisi</summary>
        [JsonPropertyName("sys")]
        public SysData Sys { get; set; } = new();

        /// <summary>Görünürlük (metre cinsinden)</summary>
        [JsonPropertyName("visibility")]
        public int Visibility { get; set; }
    }

    /// <summary>
    /// Sıcaklık, nem ve basınç verilerini tutan sınıf.
    /// </summary>
    public class MainData
    {
        /// <summary>Mevcut sıcaklık (°C)</summary>
        [JsonPropertyName("temp")]
        public double Temperature { get; set; }

        /// <summary>Hissedilen sıcaklık (°C)</summary>
        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }

        /// <summary>Minimum sıcaklık (°C)</summary>
        [JsonPropertyName("temp_min")]
        public double TempMin { get; set; }

        /// <summary>Maksimum sıcaklık (°C)</summary>
        [JsonPropertyName("temp_max")]
        public double TempMax { get; set; }

        /// <summary>Atmosfer basıncı (hPa)</summary>
        [JsonPropertyName("pressure")]
        public int Pressure { get; set; }

        /// <summary>Nem oranı (%)</summary>
        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }

    /// <summary>
    /// Rüzgar hızı ve yönü bilgilerini tutan sınıf.
    /// </summary>
    public class WindData
    {
        /// <summary>Rüzgar hızı (m/s)</summary>
        [JsonPropertyName("speed")]
        public double Speed { get; set; }

        /// <summary>Rüzgar yönü (derece)</summary>
        [JsonPropertyName("deg")]
        public int Degree { get; set; }
    }

    /// <summary>
    /// Hava durumu açıklamasını ve simgesini tutan sınıf.
    /// </summary>
    public class WeatherDescription
    {
        /// <summary>Kısa açıklama (örn. "Clouds")</summary>
        [JsonPropertyName("main")]
        public string Main { get; set; } = string.Empty;

        /// <summary>Detaylı açıklama (örn. "overcast clouds")</summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>Hava durumu ikon kodu</summary>
        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;
    }

    /// <summary>
    /// Ülke kodu ve gün doğumu/batımı verilerini tutan sınıf.
    /// </summary>
    public class SysData
    {
        /// <summary>Ülke kodu (örn. "TR")</summary>
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        /// <summary>Gün doğumu zamanı (Unix timestamp)</summary>
        [JsonPropertyName("sunrise")]
        public long Sunrise { get; set; }

        /// <summary>Gün batımı zamanı (Unix timestamp)</summary>
        [JsonPropertyName("sunset")]
        public long Sunset { get; set; }
    }
}
