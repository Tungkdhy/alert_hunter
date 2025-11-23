# Hướng dẫn cài đặt SQLite cho .NET Framework 4.5

## Cách 1: Cài đặt qua NuGet Package Manager (Khuyến nghị)

1. Mở Visual Studio
2. Click chuột phải vào project → **Manage NuGet Packages...**
3. Chọn tab **Browse**
4. Tìm kiếm: `System.Data.SQLite`
5. Chọn version **1.0.112.1** (hoặc version tương thích với .NET 4.5)
6. Click **Install**

## Cách 2: Cài đặt qua Package Manager Console

Mở **Package Manager Console** (Tools → NuGet Package Manager → Package Manager Console) và chạy:

```powershell
Install-Package System.Data.SQLite -Version 1.0.112.1
```

## Cách 3: Tải thủ công

Nếu NuGet không hoạt động, bạn có thể:

1. Tải từ: https://www.nuget.org/packages/System.Data.SQLite/1.0.112.1
2. Giải nén file `.nupkg` (đổi đuôi thành `.zip`)
3. Copy `System.Data.SQLite.dll` từ thư mục `lib\net45\` vào thư mục `packages\System.Data.SQLite.1.0.112.1\lib\net45\`
4. Copy các file native DLLs (SQLite.Interop.dll) từ `build\net45\x86\` và `x64\` vào thư mục tương ứng

## Lưu ý quan trọng

- Với .NET Framework 4.5, nên dùng version **1.0.112.1** hoặc cũ hơn
- Đảm bảo có cả file `System.Data.SQLite.dll` và `SQLite.Interop.dll` (x86/x64)
- Nếu gặp lỗi "Unable to load DLL 'SQLite.Interop.dll'", cần copy file `SQLite.Interop.dll` vào thư mục `bin\Debug\` hoặc `bin\Release\`

## Kiểm tra sau khi cài đặt

Sau khi cài đặt, rebuild project và kiểm tra:
- File `System.Data.SQLite.dll` có trong thư mục `packages\`
- Project có thể build thành công
- Không có lỗi missing reference


