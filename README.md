# 🌤️ myWeatherApplication
 
A simple, clean **desktop weather application** made in **C# Windows Forms** as a learning / portfolio project.  
Search any city in the world, get real-time weather data, and view conditions, temperature, wind, pressure, and more — all powered by the OpenWeatherMap API.
 
## 📑 Table of Contents
- [Project Overview](#project-overview)
- [Key Features](#key-features)
- [Technologies & Approach](#technologies--approach)
- [Prerequisites](#prerequisites)
- [Setup Instructions](#setup-instructions)
- [How to Use](#how-to-use)
- [Important Notes](#important-notes)
## 🌐 Project Overview
 
This is a lightweight, educational **weather desktop app** built with C# Windows Forms.  
You type a city name into the search box, select from the autocomplete suggestions, and the app fetches live weather data from the OpenWeatherMap API and displays it instantly.
 
The project demonstrates REST API consumption, JSON deserialization, async UI patterns, dynamic control rendering, and clean Windows Forms design.
 
## ✨ Key Features
 
- City name search with real-time results
- Live autocomplete dropdown as you type (OpenWeatherMap Geocoding API)
- Current, minimum, and maximum temperature display in °C
- Weather condition label and detailed description
- Dynamic weather icon loaded from OpenWeatherMap
- Localised sunrise and sunset times
- Wind speed and atmospheric pressure readings
- Error handling with user-friendly messages
## 🛠️ Technologies & Approach
 
**Core Language & Framework**
- C#
- .NET Framework — Windows Forms (WinForms)
**Main Techniques & Structures**
- **WebClient** → HTTP requests to OpenWeatherMap REST API
- **Newtonsoft.Json** → JSON deserialization into typed model classes
- **System.Windows.Forms.Timer** → debounce timer for autocomplete (400 ms delay)
- **ListBox** → dynamically positioned suggestion dropdown
- **WeatherInfo.cs** → clean model classes (`root`, `main`, `weather`, `wind`, `sys`, `coord`)
- **XAML-free** — pure WinForms with Designer layout
- **units=metric** query param — all temperatures returned in °C
These choices keep the project simple, understandable, and easy to extend.
 
## 📋 Prerequisites
 
- **Operating System**: Windows 10 / 11
- **.NET Framework**: 4.7.2 or later
- **IDE** (recommended):
  - Visual Studio 2019 or later
- **NuGet Package**:
  - `Newtonsoft.Json`
- **API Key**:
  - Free key from [openweathermap.org](https://openweathermap.org/api)
## 🚀 Setup Instructions
 
### Option 1: Clone from GitHub
 
1. Copy the repository URL
   `https://github.com/Kashvir-Sewpersad/myWeatherApplication.git`
2. In Visual Studio:
   - File → Clone Repository…
3. Paste the URL and clone
### Option 2: Download ZIP
 
1. Download ZIP from GitHub
2. Extract to a folder (e.g. `C:\Projects\myWeatherApplication`)
3. Open the `.sln` file in Visual Studio
### Run the Project
 
1. Open the solution (`myWeatherApplication.sln`)
2. Right-click the solution → **Restore NuGet Packages**
3. Open `Form1.cs` and replace the API key placeholder:
   ```csharp
   string APIKey = "YOUR_API_KEY_HERE";
   ```
4. Set `myWeatherApplication` as the **Startup Project**
5. Press **F5** or click **Run**
The application window will launch immediately.
 
## 🎯 How to Use
 
- Type a city name into the search box
- Select a suggestion from the autocomplete dropdown, or press **Enter**
- Live weather data for that city loads instantly
- All temperatures are displayed in °C
- Sunrise and sunset times are shown in your local timezone
## ⚠️ Important Notes
 
- A valid [OpenWeatherMap API key](https://openweathermap.org/api) is required (free tier is sufficient)
- Two API endpoints are used — both covered by the same free key:
  - `/data/2.5/weather` → current weather data
  - `/geo/1.0/direct` → city autocomplete suggestions
- The autocomplete debounce timer waits 400 ms after typing stops before making a request
- Collision between the suggestion dropdown and other controls is handled by calling `BringToFront()`
- Labels `labTempMin` and `labTempMax` must exist in `Form1.Designer.cs` for the project to build
---
 
© Kashvir Sewpersad 2025/2026  
https://github.com/Kashvir-Sewpersad/myWeatherApplication
