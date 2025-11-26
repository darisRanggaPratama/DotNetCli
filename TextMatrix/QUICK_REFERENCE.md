# 🎬 Quick Reference Guide

## 🚀 Start in 30 Seconds

```powershell
# 1. Navigate to project
cd D:\up2github\DotNetCli\TextMatrix

# 2. Build & Run
dotnet run

# 3. In menu, press 1, then type your text
```

---

## 📋 Menu Cheat Sheet

```
Main Menu:
┌─────────────────────────────────┐
│ 1. Animate Text (Quick)         │
│ 2. Settings                     │
│ 3. Exit                         │
└─────────────────────────────────┘

Settings Menu:
┌─────────────────────────────────┐
│ 1. Change Duration              │
│ 2. Change Frame Delay           │
│ 3. Change Speed                 │
│ 4. Change Trail Length          │
│ 5. Change Density               │
│ 6. Reset to Defaults            │
│ 7. Back to Menu                 │
└─────────────────────────────────┘
```

---

## ⌨️ Keyboard Shortcuts

| Key | Function | Context |
|-----|----------|---------|
| **1-3** | Select menu option | Main Menu |
| **1-7** | Select settings option | Settings Menu |
| **ESC** | Stop animation early | During Animation |
| **Enter** | Confirm input | Any input prompt |
| **Any Key** | Continue to menu | After animation |

---

## ⚙️ Parameter Guide

### Duration (Berapa lama animasi)
```
1000 ms  = 1 second (cepat)
5000 ms  = 5 seconds (default, balanced)
10000 ms = 10 seconds (lama)
15000 ms = 15 seconds (sangat lama)
```

### Frame Delay (Kecepatan render)
```
20 ms    = 50 FPS (sangat smooth, CPU heavy)
50 ms    = 20 FPS (default, balanced)
80 ms    = 12 FPS (slow motion effect)
100 ms   = 10 FPS (very slow)
```

### Speed (Kecepatan kolom jatuh)
```
1 = sangat lambat (1 pixel per frame)
2 = lambat (default, balanced)
3 = sedang (3 pixels per frame)
4 = cepat (4 pixels per frame)
5 = sangat cepat (5 pixels per frame)
```

### Trail Length (Panjang ekor)
```
5  = pendek (minimal trail effect)
10 = sedang (default, classic Matrix look)
15 = panjang (dramatic effect)
20 = sangat panjang (heavy trail)
```

### Density (Kepadatan kolom)
```
1 = jarang (1 kolom setiap 2 chars width)
2 = sedang (1 kolom setiap 1 char width)
3 = rapat (banyak kolom)
4 = sangat rapat
5 = ekstrem rapat (rain effect)
```

---

## 🎯 Preset Recommendations

### 🎬 Matrix Classic (Default)
```
Duration: 5000 ms
Frame Delay: 50 ms
Speed: 2
Trail Length: 10
Density: 1

Feel: Seperti scene asli dari film Matrix
Best For: General purpose, balanced
```

### ⚡ Fast & Furious
```
Duration: 3000 ms
Frame Delay: 20 ms
Speed: 4
Trail Length: 8
Density: 2

Feel: Action-packed, dramatic, fast-paced
Best For: Impressive demos, attention-grabbing
```

### 🌊 Smooth Waterfall
```
Duration: 8000 ms
Frame Delay: 80 ms
Speed: 1
Trail Length: 15
Density: 1

Feel: Slow, mesmerizing, zen-like
Best For: Presentations, background effect
```

### 🌧️ Rain of Code
```
Duration: 6000 ms
Frame Delay: 30 ms
Speed: 3
Trail Length: 12
Density: 5

Feel: Heavy, intense, chaotic
Best For: Hacker aesthetic, cyberpunk style
```

### 🎯 Quick Burst
```
Duration: 2000 ms
Frame Delay: 40 ms
Speed: 5
Trail Length: 6
Density: 2

Feel: Quick, energetic, explosive
Best For: Quick animations, transitions
```

---

## 🎨 Color Guide

```
█ Bright Green (Lime)    = Karakter di depan trail (paling terang)
█ Medium Green           = Karakter di tengah trail
█ Dark Green             = Karakter di belakang trail (paling gelap)

[White on Dark Green]    = Teks input saat glow aktif
[Bright Green on Black]  = Teks input saat glow tidak aktif
```

---

## 🐛 Quick Troubleshooting

| Problem | Solution |
|---------|----------|
| Animasi patah-patah | Ubah Frame Delay ke 30-50ms |
| Teks input tidak terlihat | Tingkatkan Trail Length |
| Kolom bergerak terlalu lambat | Tingkatkan Speed atau kurangi Frame Delay |
| Kolom bergerak terlalu cepat | Kurangi Speed atau tingkatkan Frame Delay |
| Console terlalu penuh | Kurangi Density |
| Animasi selesai terlalu cepat | Naikkan Duration |
| "head" is not recognized | Gunakan PowerShell bukan CMD |
| Aplikasi crash saat run | Perlebar jendela console minimal 80 chars |

---

## 💻 Development Commands

```powershell
# Build debug
dotnet build

# Build release
dotnet build --configuration Release

# Run debug
dotnet run

# Run release
dotnet run --configuration Release

# Clean build artifacts
dotnet clean

# Publish standalone
dotnet publish -c Release -r win-x64 --self-contained

# Watch mode (rebuild on file change)
dotnet watch run
```

---

## 📊 Performance Tips

### Untuk Smoothness
- Frame Delay: 30-50ms
- Console width: 120+ characters
- Terminal: Windows Terminal (lebih smooth dari CMD)

### Untuk Performance
- Frame Delay: 50-100ms
- Density: 1-2
- Trail Length: 8-10

### Untuk Spektakuler Effect
- Density: 4-5
- Trail Length: 12-15
- Speed: 3-4

---

## 🎮 Example Workflows

### Workflow 1: Quick Demo
```
1. Run: dotnet run
2. Press: 1
3. Type: DEMO
4. Watch animation
5. Press: Any key
6. Press: 3 to exit
```

### Workflow 2: Customize & Animate
```
1. Run: dotnet run
2. Press: 2 (Settings)
3. Press: 3 (Change Speed)
4. Type: 4
5. Press: 7 (Back)
6. Press: 1 (Animate)
7. Type: YOUR_TEXT
8. Watch animation
```

### Workflow 3: Find Perfect Settings
```
1. Press: 2 (Settings)
2. Adjust parameter (misal: 2 untuk Frame Delay)
3. Type: 30
4. Press: 6 jika perlu reset
5. Press: 7 (Back)
6. Press: 1 untuk test
7. Type: TEST
8. Repeat sampai puas
```

---

## 📈 Text Length Recommendations

```
Text Length   | Best Density | Best Trail Length | Best Duration
4-6 chars     | 1-2          | 8-10              | 3000-5000 ms
6-10 chars    | 1            | 10-12             | 5000-7000 ms
10+ chars     | 1            | 12-15             | 7000-10000 ms
```

---

## 🎬 Movie Scene Recreation

### Opening Credits Style
```
Settings:
Duration: 8000 ms
Frame Delay: 50 ms
Speed: 2
Trail Length: 12
Density: 2

Effect: Dramatic, impressive, slow build-up
```

### Action Scene
```
Settings:
Duration: 3000 ms
Frame Delay: 20 ms
Speed: 5
Trail Length: 6
Density: 5

Effect: Fast, intense, chaotic
```

### Meditation Scene
```
Settings:
Duration: 10000 ms
Frame Delay: 100 ms
Speed: 1
Trail Length: 15
Density: 1

Effect: Calm, peaceful, slow
```

---

## 🔗 File Locations

```
Project Root: D:\up2github\DotNetCli\TextMatrix\

Build Artifacts:
  Debug: bin\Debug\net8.0\TextMatrix.dll
  Release: bin\Release\net8.0\TextMatrix.dll

Executable:
  Debug: bin\Debug\net8.0\TextMatrix.exe
  Release: bin\Release\net8.0\TextMatrix.exe

Documentation:
  README.md                 ← Getting started
  FEATURES.md               ← Detailed features
  BUILD.md                  ← Build & deploy
  PROJECT_SUMMARY.md        ← Project overview
  QUICK_REFERENCE.md        ← This file
```

---

## 🎓 Learning Resources

- **Spectre.Console Docs**: https://spectreconsole.net
- **.NET Documentation**: https://docs.microsoft.com/dotnet
- **C# Documentation**: https://docs.microsoft.com/dotnet/csharp
- **Console Coding**: Classic game development techniques

---

## 🚀 Next Steps

1. **Try it**: `dotnet run`
2. **Explore**: Test berbagai kombinasi settings
3. **Customize**: Modifikasi colors atau efek di source code
4. **Share**: Tunjukkan ke teman atau kolega
5. **Enhance**: Tambahkan fitur baru sesuai kreatifitas

---

**Happy Animating! 🎬✨**

*Last Updated: November 26, 2025*

