using System.Text.Json;
using WeatherApp.Configuration;
using WeatherApp.Interfaces;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    /// <summary>
    /// Arama geçmişini JSON dosyasına okuyup yazan servis implementasyonu.
    /// 
    /// SOLID - Single Responsibility Principle (SRP):
    /// Yalnızca dosya I/O ve geçmiş yönetiminden sorumludur.
    /// Hava durumu API çağrısı veya ekran gösterimi bu sınıfın işi değildir.
    /// 
    /// SOLID - Open/Closed Principle (OCP):
    /// Gelecekte dosya yerine veritabanı veya cloud storage kullanılmak istenirse
    /// yalnızca IHistoryService'in yeni bir implementasyonu yazılır,
    /// bu sınıf değiştirilmez.
    /// </summary>
    public class HistoryService : IHistoryService
    {
        private readonly string _filePath;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true, // Okunabilir JSON formatı
            PropertyNameCaseInsensitive = true
        };

        public HistoryService(string? filePath = null)
        {
            // Test ortamında özel dosya yolu verilebilir; varsayılan AppSettings'ten gelir
            _filePath = filePath ?? AppSettings.HistoryFilePath;
            EnsureDirectoryExists();
        }

        /// <inheritdoc />
        public async Task<SearchHistory> LoadHistoryAsync()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new SearchHistory();

                var json = await File.ReadAllTextAsync(_filePath);

                if (string.IsNullOrWhiteSpace(json))
                    return new SearchHistory();

                return JsonSerializer.Deserialize<SearchHistory>(json, _jsonOptions)
                       ?? new SearchHistory();
            }
            catch (Exception ex) when (ex is IOException or JsonException)
            {
                // Dosya bozuksa sessizce yeni bir geçmiş döndür
                Console.WriteLine($"[Uyarı] Geçmiş dosyası okunamadı: {ex.Message}");
                return new SearchHistory();
            }
        }

        /// <inheritdoc />
        public async Task SaveSearchAsync(string cityName, double temperature)
        {
            try
            {
                var history = await LoadHistoryAsync();

                // Aynı şehir varsa listeden kaldır (duplicate önleme)
                history.Entries.RemoveAll(e =>
                    string.Equals(e.CityName, cityName, StringComparison.OrdinalIgnoreCase));

                // Yeni kaydı listenin başına ekle (en son aranan en üstte)
                history.Entries.Insert(0, new SearchEntry
                {
                    CityName = cityName,
                    SearchedAt = DateTime.Now,
                    LastTemperature = temperature
                });

                // Maksimum kayıt sayısını aşıyorsa en eskiyi sil
                if (history.Entries.Count > AppSettings.MaxHistoryCount)
                    history.Entries = history.Entries.Take(AppSettings.MaxHistoryCount).ToList();

                var json = JsonSerializer.Serialize(history, _jsonOptions);
                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"[Uyarı] Geçmiş kaydedilemedi: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task ClearHistoryAsync()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(new SearchHistory(), _jsonOptions));
                    Console.WriteLine("Arama geçmişi temizlendi.");
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"[Uyarı] Geçmiş temizlenemedi: {ex.Message}");
            }
        }

        // ── Yardımcı Metotlar ─────────────────────────────────────────────────

        /// <summary>
        /// Dosya yolundaki klasörler yoksa oluşturur.
        /// </summary>
        private void EnsureDirectoryExists()
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }
    }
}
