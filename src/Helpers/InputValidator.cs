using System.Text.RegularExpressions;

namespace WeatherApp.Helpers
{
    /// <summary>
    /// Kullanıcıdan alınan girdilerin doğrulanmasından sorumlu yardımcı sınıf.
    /// 
    /// SOLID - Single Responsibility Principle (SRP):
    /// Yalnızca giriş doğrulama mantığını barındırır.
    /// </summary>
    public static class InputValidator
    {
        // Geçerli şehir adı regex'i: Harf, boşluk, tire ve Türkçe karakterlere izin verir
        private static readonly Regex CityNameRegex = new(
            @"^[\p{L}\s\-\.]{2,100}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Özel komutları kontrol eder.
        /// </summary>
        public static bool IsExitCommand(string input) =>
            input.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
            input.Equals("q", StringComparison.OrdinalIgnoreCase) ||
            input.Equals("quit", StringComparison.OrdinalIgnoreCase);

        public static bool IsHistoryCommand(string input) =>
            input.Equals("history", StringComparison.OrdinalIgnoreCase) ||
            input.Equals("gecmis", StringComparison.OrdinalIgnoreCase);

        public static bool IsClearCommand(string input) =>
            input.Equals("clear", StringComparison.OrdinalIgnoreCase) ||
            input.Equals("temizle", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Şehir adının geçerli formatta olup olmadığını doğrular.
        /// </summary>
        /// <param name="cityName">Doğrulanacak şehir adı</param>
        /// <param name="errorMessage">Hata varsa mesaj</param>
        /// <returns>Geçerliyse true</returns>
        public static bool ValidateCityName(string cityName, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(cityName))
            {
                errorMessage = "Şehir adı boş olamaz. Lütfen bir şehir adı girin.";
                return false;
            }

            if (cityName.Trim().Length < 2)
            {
                errorMessage = "Şehir adı en az 2 karakter olmalıdır.";
                return false;
            }

            if (cityName.Trim().Length > 100)
            {
                errorMessage = "Şehir adı çok uzun (max 100 karakter).";
                return false;
            }

            if (!CityNameRegex.IsMatch(cityName.Trim()))
            {
                errorMessage = "Şehir adı yalnızca harf, boşluk ve tire içerebilir.";
                return false;
            }

            return true;
        }
    }
}
