namespace WeatherApp.Models
{
    /// <summary>
    /// Servis katmanından dönen sonucu temsil eden generic Result pattern sınıfı.
    /// Hem başarı hem de hata durumlarını tek bir yapıda taşır (Railway-Oriented Programming).
    /// </summary>
    /// <typeparam name="T">Başarı durumunda döndürülecek veri tipi</typeparam>
    public class WeatherResult<T>
    {
        /// <summary>İşlemin başarılı olup olmadığını gösterir</summary>
        public bool IsSuccess { get; private set; }

        /// <summary>Başarı durumunda içerik verisi</summary>
        public T? Data { get; private set; }

        /// <summary>Hata durumunda mesaj</summary>
        public string ErrorMessage { get; private set; } = string.Empty;

        // Private constructor — nesne yalnızca factory metodlarla oluşturulur
        private WeatherResult() { }

        /// <summary>
        /// Başarılı sonuç oluşturur.
        /// </summary>
        public static WeatherResult<T> Success(T data) =>
            new WeatherResult<T> { IsSuccess = true, Data = data };

        /// <summary>
        /// Hata sonucu oluşturur.
        /// </summary>
        public static WeatherResult<T> Failure(string errorMessage) =>
            new WeatherResult<T> { IsSuccess = false, ErrorMessage = errorMessage };
    }

    /// <summary>
    /// Son aranan şehirlerin geçmişini tutan model.
    /// </summary>
    public class SearchHistory
    {
        /// <summary>Son aranan şehirlerin listesi (max 5 adet)</summary>
        public List<SearchEntry> Entries { get; set; } = new();
    }

    /// <summary>
    /// Tek bir arama kaydını temsil eder.
    /// </summary>
    public class SearchEntry
    {
        /// <summary>Aranan şehir adı</summary>
        public string CityName { get; set; } = string.Empty;

        /// <summary>Arama tarihi ve saati</summary>
        public DateTime SearchedAt { get; set; }

        /// <summary>Son aranan sıcaklık değeri (°C)</summary>
        public double LastTemperature { get; set; }
    }
}
