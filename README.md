#  WeatherApp — C# Hava Durumu Uygulaması

[![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![OpenWeatherMap](https://img.shields.io/badge/API-OpenWeatherMap-orange)](https://openweathermap.org/api)

**OpenWeatherMap API** kullanarak herhangi bir şehir için anlık hava durumu bilgisi gösteren, SOLID prensiplerine uygun geliştirilmiş bir C# konsol uygulaması.

---

##  Ekran Görüntüleri

> Aşağıya `screenshots/` klasöründen ekran görüntüsü ekleyin.

```
╔══════════════════════════════════════════════════╗
║         🌤  HAVA DURUMU UYGULAMASI  🌤           ║
╚══════════════════════════════════════════════════╝

   Şehir adı girin: Istanbul

┌─────────────────────────────────────────────────┐
│  ☁️   ISTANBUL, TR                               │
├─────────────────────────────────────────────────┤
│  Durum         : Parçalı bulutlu                 │
│  Sıcaklık      : 24.3 °C                        │
│  Hissedilen    : 23.8 °C                        │
│  Min / Maks    : 21.0 °C / 27.5 °C             │
├─────────────────────────────────────────────────┤
│  Nem           : %65                            │
│  Basınç        : 1013 hPa                       │
│  Görünürlük    : 10.0 km                        │
├─────────────────────────────────────────────────┤
│  Rüzgar Hızı   : 3.5 m/s (12.6 km/h)           │
│  Rüzgar Yönü   : Kuzeybatı (NW)                 │
├─────────────────────────────────────────────────┤
│  Gün Doğumu 🌅  : 05:47                         │
│  Gün Batımı 🌇  : 20:32                         │
└─────────────────────────────────────────────────┘
```

---

##  Özellikler

| Özellik | Açıklama |
|---|---|
| 🌡️ Anlık Hava Durumu | Sıcaklık, hissedilen sıcaklık, min/maks |
| 💧 Atmosfer Bilgileri | Nem, basınç, görünürlük |
| 🌬️ Rüzgar Bilgisi | Hız (m/s ve km/h) ve yön |
| 🌅 Gün Doğumu/Batımı | Yerel saate göre hesaplanmış |
| 📋 Arama Geçmişi | Son 5 aranan şehir JSON dosyasına kaydedilir |
| ❌ Hata Yönetimi | Şehir bulunamadı, ağ hatası, zaman aşımı gibi durumlar |
| 🎨 Renkli Konsol | Veriler renk kodlamasıyla okunabilir şekilde gösterilir |

---

##  Kullanılan Teknolojiler

- **Dil**: C# 12
- **Framework**: .NET 8
- **HTTP İstemcisi**: `HttpClient` + `IHttpClientFactory`
- **JSON**: `System.Text.Json`
- **Bağımlılık Enjeksiyonu**: `Microsoft.Extensions.DependencyInjection`
- **API**: [OpenWeatherMap Current Weather API](https://openweathermap.org/current)

---

## 📁 Proje Yapısı

```
WeatherApp/
├── 📄 Program.cs                    # Giriş noktası, DI kurulumu, ana döngü
├── 📄 WeatherApp.csproj             # Proje ve paket yapılandırması
├── 📄 .gitignore
├── 📄 README.md
│
├── 📁 src/
│   ├── 📁 Configuration/
│   │   └── AppSettings.cs           # API anahtarı ve uygulama sabitleri
│   │
│   ├── 📁 Models/
│   │   ├── WeatherResponse.cs       # API JSON → C# deserialize modeli
│   │   └── WeatherResult.cs         # Result pattern + SearchHistory modeli
│   │
│   ├── 📁 Interfaces/
│   │   ├── IWeatherService.cs       # Hava durumu servisi sözleşmesi
│   │   └── IHistoryService.cs       # Geçmiş servisi sözleşmesi
│   │
│   ├── 📁 Services/
│   │   ├── WeatherService.cs        # OpenWeatherMap API implementasyonu
│   │   └── HistoryService.cs        # JSON dosya tabanlı geçmiş yönetimi
│   │
│   └── 📁 Helpers/
│       ├── ConsoleDisplayHelper.cs  # Renkli konsol çıktısı
│       └── InputValidator.cs        # Kullanıcı girdi doğrulama
│
└── 📁 data/
    └── search_history.json          # Otomatik oluşturulan geçmiş dosyası
```

---

##  Kurulum ve Çalıştırma

### Gereksinimler

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [OpenWeatherMap ücretsiz API anahtarı](https://home.openweathermap.org/users/sign_up)

### 1. Projeyi Klonlayın

```bash
git clone https://github.com/KULLANICI_ADINIZ/WeatherApp.git
cd WeatherApp
```

### 2. API Anahtarını Ayarlayın

`src/Configuration/AppSettings.cs` dosyasını açın ve API anahtarınızı girin:

```csharp
public const string ApiKey = "buraya_api_anahtarinizi_yazin";
```

> ** Güvenlik İpucu:** Gerçek projede API anahtarını ortam değişkeniyle kullanın:
> ```bash
> export OWM_API_KEY="your_key_here"
> ```
> Kod içinde: `Environment.GetEnvironmentVariable("OWM_API_KEY")`

### 3. Bağımlılıkları Yükleyin

```bash
dotnet restore
```

### 4. Uygulamayı Çalıştırın

```bash
dotnet run
```

### Yayınlama (Opsiyonel)

```bash
# Windows için tek dosya
dotnet publish -c Release -r win-x64 --self-contained

# Linux için
dotnet publish -c Release -r linux-x64 --self-contained

# macOS için
dotnet publish -c Release -r osx-x64 --self-contained
```

---

##  Kullanım

Uygulama çalıştıktan sonra şehir adını yazıp Enter'a basın:

```
 Şehir adı girin: Ankara
 Şehir adı girin: London
 Şehir adı girin: New York
```

### Özel Komutlar

| Komut | İşlev |
|---|---|
| `history` veya `gecmis` | Son 5 aranan şehri listeler |
| `clear` veya `temizle` | Arama geçmişini temizler |
| `exit`, `q` veya `quit` | Uygulamadan çıkar |

---

##  Mimari ve Tasarım Kararları

Bu proje **SOLID** prensiplerine uygun geliştirilmiştir:

| Prensip | Uygulama |
|---|---|
| **S** — Single Responsibility | Her sınıf tek bir sorumluluğa sahip (WeatherService API, HistoryService dosya, ConsoleDisplayHelper ekran) |
| **O** — Open/Closed | Geçmiş kaydı için yeni depolama (veritabanı, cloud) eklenmek istenirse `IHistoryService`'in yeni implementasyonu yazılır |
| **L** — Liskov Substitution | `IWeatherService` ve `IHistoryService` implementasyonları birbirinin yerine geçebilir (test mock'ları) |
| **I** — Interface Segregation | Hava durumu ve geçmiş servisleri ayrı arayüzlerde tanımlandı |
| **D** — Dependency Inversion | `Program.cs` somut sınıflara değil, arayüzlere bağımlı |

### Hata Yönetimi

`WeatherResult<T>` sınıfı ile **Result Pattern** uygulanmıştır. Exception'lar servis içinde yakalanır ve anlamlı hata mesajlarına dönüştürülür.

---

##  Geliştirme Aşamaları

```
Aşama 1: Temel Yapı
  ├── Proje oluşturma (dotnet new console)
  ├── Klasör yapısı kurulumu
  └── .gitignore ve README şablonu

Aşama 2: Modeller
  ├── WeatherResponse (API JSON modeli)
  └── WeatherResult (Result Pattern)

Aşama 3: Servis Katmanı
  ├── IWeatherService interface
  ├── WeatherService (HttpClient + JSON parse)
  └── Hata yönetimi (404, 401, timeout)

Aşama 4: Geçmiş Özelliği
  ├── SearchHistory modeli
  ├── IHistoryService interface
  └── HistoryService (JSON dosya okuma/yazma)

Aşama 5: Kullanıcı Arayüzü
  ├── ConsoleDisplayHelper (renkli tablo)
  ├── InputValidator
  └── Program.cs ana döngü

Aşama 6: DI Entegrasyonu
  └── Microsoft.Extensions.DependencyInjection kurulumu

Aşama 7: Test ve Dokümantasyon
  ├── Edge case testleri
  └── README güncellemesi
```

---

##  Önerilen Git Commit Mesajları

```bash
git commit -m "feat: initial project structure with SOLID architecture"
git commit -m "feat: add WeatherResponse and WeatherResult models"
git commit -m "feat: implement IWeatherService with OpenWeatherMap integration"
git commit -m "feat: add HttpClient error handling (404, 401, timeout)"
git commit -m "feat: implement search history with JSON file persistence"
git commit -m "feat: add colorized console display with weather emojis"
git commit -m "feat: add input validation and special commands (history, clear)"
git commit -m "chore: configure DI container with IHttpClientFactory"
git commit -m "docs: add comprehensive README with setup instructions"
git commit -m "refactor: extract wind direction logic to helper method"
git commit -m "fix: handle duplicate city entries in search history"
```

---

##  Gelecek Geliştirmeler

- [ ] 5 günlük hava tahmini (Forecast API)
- [ ] Şehir koordinatına göre arama (Geocoding API)
- [ ] `appsettings.json` ile konfigürasyon yönetimi
- [ ] Unit testler (xUnit + Moq)
- [ ] Fahrenheit/Celsius geçiş seçeneği
- [ ] ASCII sanat hava durumu görselleri

---

##  Lisans

Bu proje [MIT Lisansı](LICENSE) ile lisanslanmıştır.

---

##  Teşekkürler

- [OpenWeatherMap](https://openweathermap.org/) — Ücretsiz hava durumu API'si için
- [Microsoft .NET](https://dotnet.microsoft.com/) — Harika geliştirici ekosistemi için
