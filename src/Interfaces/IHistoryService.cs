using WeatherApp.Models;

namespace WeatherApp.Interfaces
{
    /// <summary>
    /// Arama geçmişi yönetiminin sözleşmesini tanımlayan arayüz.
    /// 
    /// SOLID - Interface Segregation Principle (ISP):
    /// Geçmiş yönetimi kendi arayüzünde ayrıştırılmıştır.
    /// Böylece geçmişe ihtiyaç duymayan bileşenler gereksiz bağımlılık taşımaz.
    /// </summary>
    public interface IHistoryService
    {
        /// <summary>
        /// Mevcut arama geçmişini dosyadan yükler.
        /// </summary>
        Task<SearchHistory> LoadHistoryAsync();

        /// <summary>
        /// Yeni bir şehir aramasını geçmişe ekler ve dosyaya kaydeder.
        /// Eğer şehir zaten varsa üstüne yazar, yoksa başa ekler.
        /// Liste maksimum 5 kayıt tutar.
        /// </summary>
        /// <param name="cityName">Aranan şehir adı</param>
        /// <param name="temperature">O anki sıcaklık değeri</param>
        Task SaveSearchAsync(string cityName, double temperature);

        /// <summary>
        /// Tüm arama geçmişini temizler.
        /// </summary>
        Task ClearHistoryAsync();
    }
}
