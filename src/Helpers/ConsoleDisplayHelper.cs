using WeatherApp.Models;

namespace WeatherApp.Helpers
{
    /// <summary>
    /// Konsol çıktısını biçimlendiren ve renkli hava durumu ekranı oluşturan yardımcı sınıf.
    /// 
    /// SOLID - Single Responsibility Principle (SRP):
    /// Yalnızca görüntüleme (presentation) katmanından sorumludur.
    /// İş mantığı veya veri erişimi içermez.
    /// </summary>
    public static class ConsoleDisplayHelper
    {
        // ── Renk Sabitleri ─────────────────────────────────────────────────────
        private const ConsoleColor HeaderColor    = ConsoleColor.Cyan;
        private const ConsoleColor ValueColor     = ConsoleColor.Yellow;
        private const ConsoleColor LabelColor     = ConsoleColor.White;
        private const ConsoleColor ErrorColor     = ConsoleColor.Red;
        private const ConsoleColor SuccessColor   = ConsoleColor.Green;
        private const ConsoleColor HistoryColor   = ConsoleColor.Magenta;
        private const ConsoleColor SeparatorColor = ConsoleColor.DarkGray;

        /// <summary>
        /// Uygulama açılış banner'ını gösterir.
        /// </summary>
        public static void ShowWelcomeBanner()
        {
            Console.Clear();
            WriteColorLine(SeparatorColor, "╔══════════════════════════════════════════════════╗");
            WriteColorLine(HeaderColor,    "║         🌤  HAVA DURUMU UYGULAMASI  🌤           ║");
            WriteColorLine(SeparatorColor, "╠══════════════════════════════════════════════════╣");
            WriteColorLine(LabelColor,     "║  OpenWeatherMap API ile Güncel Hava Durumu       ║");
            WriteColorLine(LabelColor,     "║  Çıkmak için 'exit' veya 'q' yazın               ║");
            WriteColorLine(LabelColor,     "║  Geçmişi görmek için 'history' yazın             ║");
            WriteColorLine(LabelColor,     "║  Geçmişi temizlemek için 'clear' yazın           ║");
            WriteColorLine(SeparatorColor, "╚══════════════════════════════════════════════════╝");
            Console.WriteLine();
        }

        /// <summary>
        /// Hava durumu verilerini formatlı şekilde konsola yazdırır.
        /// </summary>
        /// <param name="weather">Gösterilecek hava durumu verisi</param>
        public static void ShowWeatherData(WeatherResponse weather)
        {
            var description = weather.Weather.FirstOrDefault();
            var emoji = GetWeatherEmoji(description?.Main ?? string.Empty);
            var windDirection = GetWindDirection(weather.Wind.Degree);

            Console.WriteLine();
            WriteColorLine(SeparatorColor, "┌─────────────────────────────────────────────────┐");

            // Şehir başlığı
            WriteColorLine(HeaderColor,
                $"│  {emoji}  {weather.CityName.ToUpperInvariant()}, {weather.Sys.Country}".PadRight(50) + "│");

            WriteColorLine(SeparatorColor, "├─────────────────────────────────────────────────┤");

            // Hava açıklaması
            var descText = description?.Description ?? "bilinmiyor";
            WriteColorLine(LabelColor,
                $"│  Durum         : ".PadRight(20));
            WriteInlineColor(ValueColor, CultureFormat(descText));
            Console.WriteLine();

            // Sıcaklık bilgileri
            PrintRow("Sıcaklık",       $"{weather.Main.Temperature:F1} °C");
            PrintRow("Hissedilen",      $"{weather.Main.FeelsLike:F1} °C");
            PrintRow("Min / Maks",      $"{weather.Main.TempMin:F1} °C / {weather.Main.TempMax:F1} °C");

            WriteColorLine(SeparatorColor, "├─────────────────────────────────────────────────┤");

            // Atmosfer bilgileri
            PrintRow("Nem",            $"%{weather.Main.Humidity}");
            PrintRow("Basınç",         $"{weather.Main.Pressure} hPa");
            PrintRow("Görünürlük",     $"{weather.Visibility / 1000.0:F1} km");

            WriteColorLine(SeparatorColor, "├─────────────────────────────────────────────────┤");

            // Rüzgar bilgileri
            PrintRow("Rüzgar Hızı",    $"{weather.Wind.Speed:F1} m/s ({weather.Wind.Speed * 3.6:F1} km/h)");
            PrintRow("Rüzgar Yönü",    windDirection);

            WriteColorLine(SeparatorColor, "├─────────────────────────────────────────────────┤");

            // Gün doğumu / batımı
            var sunrise = DateTimeOffset.FromUnixTimeSeconds(weather.Sys.Sunrise).LocalDateTime;
            var sunset  = DateTimeOffset.FromUnixTimeSeconds(weather.Sys.Sunset).LocalDateTime;
            PrintRow("Gün Doğumu 🌅",  sunrise.ToString("HH:mm"));
            PrintRow("Gün Batımı 🌇",  sunset.ToString("HH:mm"));

            WriteColorLine(SeparatorColor, "└─────────────────────────────────────────────────┘");
            Console.WriteLine();
        }

        /// <summary>
        /// Son aranan şehirlerin listesini gösterir.
        /// </summary>
        public static void ShowHistory(SearchHistory history)
        {
            Console.WriteLine();

            if (!history.Entries.Any())
            {
                WriteColorLine(HistoryColor, "  📋 Henüz arama geçmişi bulunmuyor.");
                Console.WriteLine();
                return;
            }

            WriteColorLine(SeparatorColor, "┌─────────────────────────────────────────────────┐");
            WriteColorLine(HistoryColor,   "│          📋 SON ARANAN ŞEHİRLER                 │");
            WriteColorLine(SeparatorColor, "├──────┬──────────────────────┬───────────────────┤");
            WriteColorLine(SeparatorColor, "│  #   │  Şehir               │  Tarih / Sıcaklık │");
            WriteColorLine(SeparatorColor, "├──────┼──────────────────────┼───────────────────┤");

            for (int i = 0; i < history.Entries.Count; i++)
            {
                var entry = history.Entries[i];
                var index = $"  {i + 1}".PadRight(4);
                var city  = entry.CityName.PadRight(20);
                var date  = $"{entry.SearchedAt:dd.MM HH:mm} / {entry.LastTemperature:F1}°C".PadRight(17);

                Console.Write("│");
                WriteInlineColor(HistoryColor, index);
                Console.Write("  │ ");
                WriteInlineColor(ValueColor, city);
                Console.Write("│ ");
                WriteInlineColor(LabelColor, date);
                Console.WriteLine(" │");
            }

            WriteColorLine(SeparatorColor, "└──────┴──────────────────────┴───────────────────┘");
            Console.WriteLine();
        }

        /// <summary>
        /// Hata mesajını kırmızı renkte gösterir.
        /// </summary>
        public static void ShowError(string message)
        {
            Console.WriteLine();
            WriteColorLine(ErrorColor, $"  ❌  {message}");
            Console.WriteLine();
        }

        /// <summary>
        /// Başarı mesajını yeşil renkte gösterir.
        /// </summary>
        public static void ShowSuccess(string message)
        {
            WriteColorLine(SuccessColor, $"  ✅  {message}");
        }

        // ── Özel Yardımcı Metotlar ─────────────────────────────────────────────

        private static void PrintRow(string label, string value)
        {
            var paddedLabel = $"│  {label}".PadRight(20);
            Console.Write(paddedLabel + ": ");
            WriteInlineColor(ValueColor, value);
            Console.WriteLine();
        }

        private static void WriteColorLine(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private static void WriteInlineColor(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        /// <summary>
        /// Hava durumu türüne göre emoji döndürür.
        /// </summary>
        private static string GetWeatherEmoji(string weatherMain) => weatherMain.ToLower() switch
        {
            "clear"        => "☀️ ",
            "clouds"       => "☁️ ",
            "rain"         => "🌧️ ",
            "drizzle"      => "🌦️ ",
            "thunderstorm" => "⛈️ ",
            "snow"         => "❄️ ",
            "mist" or "fog" or "haze" => "🌫️ ",
            _              => "🌈"
        };

        /// <summary>
        /// Derece değerini yön adına (N, NE, E...) çevirir.
        /// </summary>
        private static string GetWindDirection(int degree) => degree switch
        {
            >= 337 or < 23  => "Kuzey (N)",
            >= 23 and < 68  => "Kuzeydoğu (NE)",
            >= 68 and < 113 => "Doğu (E)",
            >= 113 and < 158 => "Güneydoğu (SE)",
            >= 158 and < 203 => "Güney (S)",
            >= 203 and < 248 => "Güneybatı (SW)",
            >= 248 and < 293 => "Batı (W)",
            _               => "Kuzeybatı (NW)"
        };

        /// <summary>
        /// String'in ilk harfini büyük yapar (Türkçe lokalizasyon için).
        /// </summary>
        private static string CultureFormat(string input) =>
            string.IsNullOrEmpty(input) ? input
            : char.ToUpper(input[0]) + input[1..];
    }
}
