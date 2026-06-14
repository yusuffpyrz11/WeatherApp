using Microsoft.Extensions.DependencyInjection;
using WeatherApp.Configuration;
using WeatherApp.Helpers;
using WeatherApp.Interfaces;
using WeatherApp.Services;

// ═══════════════════════════════════════════════════════════════════════════
//  WeatherApp — Hava Durumu Uygulaması
//  Yazar   : GitHub Kullanıcı Adınız
//  Lisans  : MIT
//  API     : https://openweathermap.org/api
// ═══════════════════════════════════════════════════════════════════════════

// ── 1. Dependency Injection Kurulumu ─────────────────────────────────────
var services = new ServiceCollection();

// HttpClient'ı IHttpClientFactory üzerinden kaydet (best practice)
services.AddHttpClient<IWeatherService, WeatherService>();

// HistoryService'i Singleton olarak kaydet (tek dosyaya erişiyor)
services.AddSingleton<IHistoryService, HistoryService>();

var serviceProvider = services.BuildServiceProvider();

// ── 2. Servisleri Resolve Et ──────────────────────────────────────────────
var weatherService = serviceProvider.GetRequiredService<IWeatherService>();
var historyService = serviceProvider.GetRequiredService<IHistoryService>();

// ── 3. Başlangıç Ekranı ───────────────────────────────────────────────────
ConsoleDisplayHelper.ShowWelcomeBanner();

// API anahtarı kontrol uyarısı
if (AppSettings.ApiKey == "YOUR_API_KEY_HERE")
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("  ⚠️  UYARI: API anahtarı ayarlanmamış!");
    Console.WriteLine("  📝  src/Configuration/AppSettings.cs dosyasında");
    Console.WriteLine("      ApiKey değerini güncelleyin.");
    Console.WriteLine("  🔗  https://openweathermap.org/api adresinden");
    Console.WriteLine("      ücretsiz API anahtarı alabilirsiniz.\n");
    Console.ResetColor();
}

// ── 4. Ana Uygulama Döngüsü ──────────────────────────────────────────────
while (true)
{
    // Kullanıcıdan şehir adı al
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("  🔍 Şehir adı girin: ");
    Console.ResetColor();

    var input = Console.ReadLine()?.Trim() ?? string.Empty;

    // Boş giriş kontrolü
    if (string.IsNullOrWhiteSpace(input))
    {
        ConsoleDisplayHelper.ShowError("Lütfen bir şehir adı girin.");
        continue;
    }

    // ── Özel Komutlar ─────────────────────────────────────────────────────

    // Çıkış komutu
    if (InputValidator.IsExitCommand(input))
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n  👋 Görüşmek üzere!\n");
        Console.ResetColor();
        break;
    }

    // Geçmiş görüntüleme komutu
    if (InputValidator.IsHistoryCommand(input))
    {
        var history = await historyService.LoadHistoryAsync();
        ConsoleDisplayHelper.ShowHistory(history);
        continue;
    }

    // Geçmiş temizleme komutu
    if (InputValidator.IsClearCommand(input))
    {
        await historyService.ClearHistoryAsync();
        ConsoleDisplayHelper.ShowSuccess("Arama geçmişi temizlendi.");
        Console.WriteLine();
        continue;
    }

    // ── Girdi Doğrulama ────────────────────────────────────────────────────
    if (!InputValidator.ValidateCityName(input, out var validationError))
    {
        ConsoleDisplayHelper.ShowError(validationError);
        continue;
    }

    // ── API Çağrısı ────────────────────────────────────────────────────────
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine($"  ⏳ '{input}' için hava durumu alınıyor...");
    Console.ResetColor();

    var result = await weatherService.GetWeatherAsync(input);

    // ── Sonuç İşleme ───────────────────────────────────────────────────────
    if (result.IsSuccess && result.Data is not null)
    {
        // Hava durumunu ekranda göster
        ConsoleDisplayHelper.ShowWeatherData(result.Data);

        // Arama geçmişine kaydet
        await historyService.SaveSearchAsync(
            result.Data.CityName,
            result.Data.Main.Temperature);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  💾 Arama geçmişe kaydedildi.");
        Console.ResetColor();
    }
    else
    {
        ConsoleDisplayHelper.ShowError(result.ErrorMessage);
    }

    Console.WriteLine();
}
