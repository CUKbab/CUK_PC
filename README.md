# CUK밥 PC (CUK Desktop)

Cross-platform Desktop application for viewing Catholic University of Korea (가톨릭대학교) cafeteria meal menus, ported from Android to Avalonia UI (.NET).

Targets:
- **Linux (x64)**
- **Windows (x64)**
- **macOS Intel (x86_64)**
- **macOS Apple Silicon (ARM64)**

---

## Features

- 🍱 **Buon Pranzo (부온 프란초 / 학생식당)**: 1,000 KRW Morning, Korean Cuisine, Global & Noodle, Plus Corner, Dinner
- ☕ **Cafe Bona (카페 보나 / 교직원식당)**: Rice Bowl (덮밥)
- 📅 **Date Navigator**: Fast date switching (Monday to Friday) + Today shortcut
- 🌐 **Multi-Language Support**: Korean (한국어), English, Japanese (日本語), Chinese (简体中文)
- 🎨 **Theme & Styling**: Light mode, Dark mode, and System default with font scaling
- 🚀 **Offline Cache**: Automatically caches menu data locally with network fallback
- 📝 **Feedback & Bug Reports**: Direct integration with reporting API & Changelog viewer

---

## Development & Running

### Requirements
- .NET 9 or .NET 10 SDK

### Run Locally (Linux)
```bash
dotnet run --project CUK/CUK.csproj
```

---

## Multi-Platform Publishing

Run the automated build script to produce self-contained single-file binaries and macOS `.app` bundles:

```bash
./publish-all.sh
```

Artifacts will be placed in `dist/`:
- `dist/linux-x64/CUK` (Single Linux binary)
- `dist/win-x64/CUK.exe` (Single Windows executable)
- `dist/osx-x64/CUK.app` (macOS Intel App bundle)
- `dist/osx-arm64/CUK.app` (macOS Apple Silicon App bundle)

### Individual Platform Publish Commands

#### Linux (x64)
```bash
dotnet publish CUK/CUK.csproj -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true -o dist/linux-x64
```

#### Windows (x64)
```bash
dotnet publish CUK/CUK.csproj -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -o dist/win-x64
```

#### macOS (Intel x86_64)
```bash
dotnet publish CUK/CUK.csproj -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true -o dist/osx-x64
```

#### macOS (Apple Silicon ARM64)
```bash
dotnet publish CUK/CUK.csproj -c Release -r osx-arm64 --self-contained -p:PublishSingleFile=true -o dist/osx-arm64
```
