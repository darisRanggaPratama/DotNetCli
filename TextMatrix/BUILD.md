# Build & Deployment Guide

## 📋 Prasyarat

### Hardware Minimum
- Processor: Intel Core 2 Duo / AMD Athlon 64 atau lebih baru
- RAM: 512 MB
- Storage: 100 MB (termasuk .NET SDK)

### Software Requirements
- **Windows**: Windows 7 SP1 atau lebih baru
- **.NET SDK**: 8.0 atau lebih baru
- **Terminal**: PowerShell 5.1+, Command Prompt, atau Windows Terminal

### Verifikasi Instalasi

```powershell
# Check .NET version
dotnet --version

# Expected output: 8.x.x atau lebih tinggi
```

## 🔧 Build dari Source

### 1. Clone/Download Project
```powershell
cd D:\up2github\DotNetCli\TextMatrix
```

### 2. Restore Dependencies
```powershell
dotnet restore
```
Output yang diharapkan:
```
Determining projects to restore...
  Restored D:\...\TextMatrix.csproj (in X.XX sec)
  Restore succeeded.
```

### 3. Build Project
```powershell
dotnet build
```
Output yang diharapkan:
```
Determining projects to restore...
  All projects are up-to-date for restore.
  TextMatrix -> D:\...\bin\Debug\net8.0\TextMatrix.dll

Build succeeded.
  0 Warning(s)
  0 Error(s)
```

### 4. Run Application
```powershell
dotnet run
```

## 📦 Build Modes

### Debug Build (Development)
```powershell
dotnet build --configuration Debug
```
- Lebih lambat
- Lebih besar file
- Include debug symbols
- Cocok untuk development

### Release Build (Production)
```powershell
dotnet build --configuration Release
```
- Lebih cepat
- Lebih kecil file
- Optimized code
- Cocok untuk distribution

```powershell
# Run release build
dotnet run --configuration Release
```

## 🚀 Publish untuk Distribution

### Publish sebagai Standalone Executable

```powershell
# Windows 10/11 x64
dotnet publish -c Release -r win-x64 --self-contained

# Hasilnya di: bin\Release\net8.0\win-x64\publish\TextMatrix.exe
```

### Self-Contained vs Framework-Dependent

**Self-Contained (Standalone)**
```powershell
dotnet publish -c Release -r win-x64 --self-contained
```
- Ukuran: ~100+ MB
- Tidak perlu .NET SDK terinstall di target machine
- Cocok untuk distribusi ke user yang tidak tech-savvy

**Framework-Dependent**
```powershell
dotnet publish -c Release --no-self-contained
```
- Ukuran: ~5-10 MB
- Perlu .NET 8 Runtime di target machine
- Cocok untuk internal distribution

## 📁 Output Directory Structure

Setelah build sukses:
```
TextMatrix/
├── bin/
│   └── Release/  (atau Debug)
│       └── net8.0/
│           ├── TextMatrix.exe          # Executable
│           ├── TextMatrix.dll          # Main library
│           └── Spectre.Console.dll     # Dependency
└── publish/      (hasil publish)
    └── win-x64/
        └── publish/
            ├── TextMatrix.exe          # Standalone exe
            ├── TextMatrix.dll
            └── (semua dependencies)
```

## ✅ Verification Checklist

Setelah build:
- [ ] Tidak ada error saat compile
- [ ] Warning minimal (idealnya 0)
- [ ] `.exe` file tergenerate di bin folder
- [ ] Application bisa di-run tanpa error
- [ ] Menu interaktif responsive
- [ ] Animasi smooth tanpa lag

## 🐛 Troubleshooting Build

### Error: "dotnet" is not recognized
**Solusi:**
- Install .NET 8 SDK dari https://dotnet.microsoft.com/download
- Restart Terminal/CMD

### Error: "TextMatrix.csproj" not found
**Solusi:**
```powershell
cd D:\up2github\DotNetCli\TextMatrix
dir  # Pastikan TextMatrix.csproj ada
```

### Error: Spectre.Console not found
**Solusi:**
```powershell
dotnet restore --force
dotnet build
```

### Application crashes saat run
**Solusi:**
1. Check console width: konsol harus minimal 80 characters wide
2. Check encoding: `chcp 65001` (UTF-8)
3. Pastikan font console support Unicode

```powershell
# Set console ke UTF-8
chcp 65001

# Jalankan aplikasi
dotnet run
```

## 🔒 Security Considerations

- Aplikasi tidak mengakses internet
- Tidak ada database/cloud services
- Data hanya disimpan di memory (volatile)
- Tidak ada file system write operations
- Safe untuk public distribution

## 📊 Build Automation

### GitHub Actions Example
```yaml
name: Build Matrix Animation

on: [push]

jobs:
  build:
    runs-on: windows-latest
    
    steps:
    - uses: actions/checkout@v3
    - uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'
    
    - run: dotnet restore
    - run: dotnet build --configuration Release
    - run: dotnet publish -c Release -r win-x64 --self-contained
    
    - uses: actions/upload-artifact@v3
      with:
        name: TextMatrix-Release
        path: bin/Release/net8.0/win-x64/publish/
```

## 📝 Project File Reference

### TextMatrix.csproj
```xml
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <OutputType>Exe</OutputType>
        <TargetFramework>net8.0</TargetFramework>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
        <!-- Tambahan untuk production -->
        <PublishReadyToRun>true</PublishReadyToRun>
        <PublishTrimmed>true</PublishTrimmed>
    </PropertyGroup>

    <ItemGroup>
        <PackageReference Include="Spectre.Console" Version="0.48.0" />
    </ItemGroup>
</Project>
```

### Properties untuk Optimization
```xml
<!-- Dalam PropertyGroup -->
<PublishReadyToRun>true</PublishReadyToRun>    <!-- Pre-compiled for faster startup -->
<PublishTrimmed>true</PublishTrimmed>          <!-- Trim unused assemblies -->
<SelfContained>true</SelfContained>            <!-- Standalone executable -->
<RuntimeIdentifier>win-x64</RuntimeIdentifier> <!-- Target Windows 64-bit -->
```

## 🎯 Recommended Build Steps untuk Production

```powershell
# 1. Clean build artifacts
dotnet clean

# 2. Restore dependencies
dotnet restore

# 3. Build dalam Release mode
dotnet build --configuration Release --no-restore

# 4. Run tests (jika ada)
# dotnet test

# 5. Publish standalone
dotnet publish -c Release -r win-x64 --no-restore `
    /p:PublishReadyToRun=true `
    /p:PublishTrimmed=true

# 6. Create zip for distribution
$date = Get-Date -Format "yyyyMMdd"
Compress-Archive -Path ".\bin\Release\net8.0\win-x64\publish\*" `
    -DestinationPath ".\TextMatrix-Release-$date.zip"

Write-Host "Build complete! File: TextMatrix-Release-$date.zip"
```

## 📦 Distribution Package

Untuk membuat package siap distribusi:

1. **Publish aplikasi:**
   ```powershell
   dotnet publish -c Release -r win-x64 --self-contained
   ```

2. **Copy ke folder baru:**
   ```powershell
   mkdir TextMatrix-Release
   Copy-Item "bin/Release/net8.0/win-x64/publish/*" -Destination "TextMatrix-Release" -Recurse
   ```

3. **Add dokumentasi:**
   ```powershell
   Copy-Item "README.md", "FEATURES.md" -Destination "TextMatrix-Release"
   ```

4. **Create archive:**
   ```powershell
   Compress-Archive -Path "TextMatrix-Release" -DestinationPath "TextMatrix-1.0.zip"
   ```

## 🔄 Continuous Build

Untuk development yang berkelanjutan:

```powershell
# Watch mode - rebuild otomatis saat file berubah
dotnet watch run
```

---

**Build & Deployment Guide v1.0**

