# LeftLovers 

> **Share Food. Reduce Waste.**

LeftLovers is a cross-platform mobile application built with .NET MAUI that helps communities share leftover food to reduce food waste.

---

##  Features

- **Browse Food Listings** — Discover available food shared by your community
- **Interactive Map** — Find nearby food on a map with location pins
- **Post Food** — Share your leftover food with photos and pickup details
- **Manage Posts** — Activate/deactivate your listings easily
- **User Accounts** — Secure authentication with Firebase

---

##  Tech Stack

| Technology | Purpose |
|------------|---------|
| **.NET MAUI** | Cross-platform UI framework |
| **C#** | Primary programming language |
| **Firebase Auth** | User authentication |
| **Firebase Firestore** | Cloud database |
| **MAUI Maps** | Interactive map integration |

---

##  Project Structure

```
LeftLovers/
├── Models/           # Data models (FoodPost, UserProfile)
├── Views/            # XAML UI pages
├── ViewModels/       # MVVM view models
├── Services/         # Firebase & app services
├── Converters/       # XAML value converters
├── Resources/        # Images, fonts, styles
└── Platforms/        # Platform-specific code
```

---

##  Getting Started

### Prerequisites

- Visual Studio 2022 (17.3+) with .NET MAUI workload
- .NET 7.0 or later
- Android SDK / Xcode (for mobile development)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/LeftLovers.git
   cd LeftLovers
   ```

2. **Open the solution**
   ```
   Open LeftLovers.sln in Visual Studio
   ```

3. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

4. **Configure Firebase**
   - Update `google-services.json` with your Firebase project credentials

5. **Run the app**
   - Select your target platform (Android/iOS/Windows)
   - Press F5 or click Run

---

##  Documentation

For detailed usage instructions, see the [User Manual](https://htmlpreview.github.io/?https://github.com/Dexufy/LeftLovers/blob/master/USER_MANUAL.html).

---

##  License

This project is for educational purposes.

---

##  Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

---

**Made with ❤️ to reduce food waste**
