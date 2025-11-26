# Matrix Text Animation Console

Sebuah aplikasi console interaktif .NET 8 yang menampilkan animasi teks mirip dengan film Matrix menggunakan Spectre.Console.

## 📋 Fitur

✨ **Animasi Matrix Realistis**
- Karakter katakana Jepang jatuh dari atas ke bawah
- Efek trail dengan gradasi warna (hijau terang → gelap)
- Teks input ditampilkan di tengah layar dengan efek glow berkedip
- Collision detection sederhana untuk mencegah overlap karakter

🎮 **Menu Interaktif**
- Pilihan untuk menjalankan animasi cepat
- Menu pengaturan lengkap
- Validasi input yang robust

⚙️ **Konfigurasi Fleksibel**
- **Duration**: Durasi animasi (1000-15000 ms)
- **Frame Delay**: Kecepatan render (20-100 ms)
- **Speed**: Kecepatan kolom jatuh (1-5)
- **Trail Length**: Panjang ekor kolom (5-20 karakter)
- **Density**: Kepadatan kolom (1-5)

## 🚀 Cara Menggunakan

### Prasyarat
- .NET 8.0 SDK atau lebih tinggi
- Terminal/Command Prompt yang mendukung ANSI colors

### Build & Run

```powershell
# Restore dependencies
dotnet restore

# Build project
dotnet build

# Run aplikasi
dotnet run
```

### Menu Utama

```
═══════════════════════════════════
1. Animate Text (Quick)    - Jalankan animasi dengan teks custom
2. Settings                - Atur parameter animasi
3. Exit                    - Keluar dari aplikasi
═══════════════════════════════════
```

### Contoh Penggunaan

1. Pilih **1** untuk memulai animasi
2. Ketik teks yang ingin dianimasikan (misal: `rangga`)
3. Tekan **ESC** untuk menghentikan animasi lebih awal
4. Atau tunggu sampai durasi berakhir

### Mengubah Pengaturan

1. Pilih **2** di menu utama
2. Pilih parameter yang ingin diubah (1-5)
3. Masukkan nilai baru
6. Animasi selanjutnya akan menggunakan pengaturan baru

## 📁 Struktur File

```
TextMatrix/
├── Program.cs                 # Entry point & menu utama
├── MatrixAnimator.cs          # Logika animasi & update state
├── MatrixColumn.cs            # Representasi kolom jatuh
├── MatrixRenderer.cs          # Rendering dengan Spectre.Console
├── AnimationConfig.cs         # Konfigurasi animasi
├── TextMatrix.csproj          # Project file dengan dependencies
└── README.md                  # Dokumentasi ini
```

## 🏗️ Arsitektur

### MatrixColumn
Merepresentasikan satu kolom karakter yang jatuh dengan:
- Posisi (X, Y)
- Array karakter untuk animasi
- Kecepatan jatuh
- Panjang trail

### MatrixAnimator
Mengatur sistem animasi keseluruhan:
- Mengelola daftar kolom jatuh
- Update posisi setiap frame
- Reset kolom saat keluar dari layar
- Support konfigurasi custom

### MatrixRenderer
Menangani visualisasi dengan Spectre.Console:
- Render kolom dengan efek trail dan gradasi warna
- Render teks input di tengah dengan efek glow
- Menggunakan markup ANSI untuk styling

### AnimationConfig
Menyimpan konfigurasi animasi:
- Duration (default: 5000ms)
- FrameDelay (default: 50ms)
- Speed (default: 2)
- TrailLength (default: 10)
- Density (default: 1)

## 🎨 Warna & Styling

- **Lime (#00FF00)**: Karakter utama kolom
- **Green (#008000)**: Karakter di tengah trail
- **DarkGreen (#000800)**: Karakter di akhir trail
- **White on DarkGreen**: Teks input saat glow aktif

## ⌨️ Keyboard Shortcuts

| Tombol | Fungsi |
|--------|--------|
| ESC | Hentikan animasi lebih awal |
| Any Key | Lanjutkan dari hasil animasi ke menu |

## 💡 Tips & Tricks

1. **Animasi Lebih Cepat**: Kurangi Frame Delay (20-30ms)
2. **Efek Lebih Rapat**: Tingkatkan Density (3-5)
3. **Trail Lebih Panjang**: Tingkatkan Trail Length (15-20)
4. **Kolom Lebih Cepat**: Tingkatkan Speed (4-5)
5. **Durasi Lebih Lama**: Tingkatkan Duration (8000-10000ms)

## 🐛 Troubleshooting

**Q: Animasi terlihat patah-patah**
A: Kurangi Frame Delay menjadi 30-40ms atau tingkatkan ke 60-70ms untuk smoothness

**Q: Teks input tidak terlihat jelas**
A: Tingkatkan Trail Length agar efek glow lebih terlihat

**Q: Kolom tidak terlihat bergerak**
A: Naikkan Speed atau kurangi Frame Delay

## 📦 Dependencies

- **Spectre.Console**: Untuk ANSI console styling dan rendering
  ```xml
  <PackageReference Include="Spectre.Console" Version="0.48.0" />
  ```

## 🔧 Development Notes

Aplikasi ini menggunakan:
- **.NET 8.0**: Target framework
- **Implicit Usings**: Untuk namespace shortcuts
- **Nullable Reference Types**: Untuk type safety
- **Pattern Matching**: Untuk clean code

## 📝 Lisensi

Gratis untuk digunakan dan dimodifikasi.

## 🙏 Credits

- Terinspirasi dari film Matrix (1999)
- Built dengan Spectre.Console library
- .NET 8.0 SDK

---

**Happy Animating! 🎬✨**

