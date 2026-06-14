namespace WeatherApp.Configuration
{
    /// <summary>
    /// Uygulama genelindeki sabit ayarları ve konfigürasyon değerlerini barındırır.
    /// Gerçek projede bu değerler appsettings.json veya environment variable'dan okunur.
    /// </summary>
    public static class AppSettings
    {
        // ─────────────────────────────────────────────────────────────────────
        // OpenWeatherMap API Ayarları
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// OpenWeatherMap API anahtarı.
        /// Ücretsiz anahtar almak için: https://home.openweathermap.org/api_keys
        /// 
        /// GÜVENLİK NOTU: Gerçek projede bu değeri kaynak koduna yazmayın!
        /// Bunun yerine: Environment.GetEnvironmentVariable("OWM_API_KEY") kullanın.
        /// </summary>
        public const string ApiKey = "YOUR_API_KEY_HERE";

        /// <summary>
        /// OpenWeatherMap güncel hava durumu endpoint'i.
        /// {0} = şehir adı, {1} = API anahtarı, {2} = birim sistemi
        /// </summary>
        public const string BaseApiUrl =
            "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units={2}&lang=tr";

        // ─────────────────────────────────────────────────────────────────────
        // Birim Sistemi
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Sıcaklık birimi:
        /// "metric"   → Celsius (°C)
        /// "imperial" → Fahrenheit (°F)
        /// "standard" → Kelvin (K)
        /// </summary>
        public const string Units = "metric";

        // ─────────────────────────────────────────────────────────────────────
        // Uygulama Ayarları
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>HttpClient için zaman aşımı süresi (saniye)</summary>
        public const int HttpTimeoutSeconds = 10;

        /// <summary>Kaydedilecek maksimum arama geçmişi sayısı</summary>
        public const int MaxHistoryCount = 5;

        /// <summary>Arama geçmişinin kaydedileceği dosya yolu</summary>
        public const string HistoryFilePath = "data/search_history.json";
    }
}
