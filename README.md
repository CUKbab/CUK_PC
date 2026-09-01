# CUK밥 PC (CUK Desktop)

Cross-platform Desktop application for viewing Catholic University of Korea (가톨릭대학교) cafeteria meal menus using Avalonia UI (.NET).

Targets:
- **Linux (x64)**
- **Windows (x64)**
- **macOS Intel (x86_64)**
- **macOS Apple Silicon (ARM64)**

---

## Features

- **Buon Pranzo (부온 프란조)**: 1,000 KRW Morning, Korean Cuisine, Global & Noodle, Plus Corner, Dinner
- **Cafe Bona (카페 보나)**: Rice Bowl (덮밥)
- **Date Navigator**: Fast date switching (Monday to Friday) + Today shortcut
- **Multi-Language Support**: Korean (한국어), English, Japanese (日本語), Chinese (简体中文)
- **Theme & Styling**: Light mode, Dark mode, and System default with font scaling
- **Offline Cache**: Automatically caches menu data locally with network fallback
- **Feedback & Bug Reports**: Direct integration with reporting API & Changelog viewer

---

## Development & Running

### Requirements
- .NET 9 or .NET 10 SDK

### Run Locally (Linux)
```bash
dotnet run --project CUK.csproj
```

---

### Individual Platform Publish Commands

#### Linux (x64)
```bash
dotnet publish CUK.csproj -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true -o dist/linux-x64
```

#### Windows (x64)
```bash
dotnet publish CUK.csproj -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -o dist/win-x64
```

#### macOS (Intel x86_64)
```bash
dotnet publish CUK.csproj -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true -o dist/osx-x64
```

#### macOS (Apple Silicon ARM64)
```bash
dotnet publish CUK.csproj -c Release -r osx-arm64 --self-contained -p:PublishSingleFile=true -o dist/osx-arm64
```
