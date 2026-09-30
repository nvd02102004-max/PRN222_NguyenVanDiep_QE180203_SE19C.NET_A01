# PRN222 Assignment 01 - Building a News Management System with ASP.NET Core MVC

## 📌 Thông tin sinh viên
- **Họ và tên:** Nguyễn Văn Điệp
- **Mã số sinh viên:** QE180203
- **Lớp:** SE19C.NET
- **Môn học:** PRN222 - C# and .NET Programming

---

## 🏗️ Kiến trúc hệ thống (3-Layers Architecture)
Solution: `NguyenVanDiep_SE19C.NET_A01.sln`

- **BusinessObjects:** Chứa các thực thể (`Category`, `NewsArticle`, `NewsTag`, `Tag`, `SystemAccount`) với DataAnnotations Validation.
- **DataAccessObjects:** `FUNewsManagementContext` (EF Core) và các DAO áp dụng **Singleton Pattern**.
- **Repositories:** Triển khai **Repository Pattern** trung gian giữa Data Access và Business Services.
- **Services:** Tầng xử lý nghiệp vụ trung gian theo kiến trúc 3 lớp chuẩn (`Controller` -> `Service` -> `Repository` -> `DAO`).
- **NguyenVanDiepMVC:** Ứng dụng Web ASP.NET Core MVC.

---

## ⚡ Các tính năng chính (Main Functions)
1. **Giao diện mặc định:** Mặc định điều hướng ngay tới giao diện **Login** khi mở ứng dụng.
2. **Popup Modal & Confirm Dialog:** Thao tác Create/Update hiển thị dạng Bootstrap Popup Modal; Thao tác Xóa tích hợp SweetAlert2 xác nhận an toàn.
3. **Phân quyền người dùng (Role-based Authorization):**
   - **Khách (Guest / Unauthenticated):** Xem danh sách tin tức Active, tìm kiếm và xem chi tiết bài viết.
   - **Lecturer (Role = 2):** Đăng nhập, xem danh sách bài viết Active.
   - **Staff (Role = 1):** 
     - Quản lý danh mục (không cho phép xóa danh mục nếu đang có bài viết tham chiếu).
     - Quản lý bài viết + gắn thẻ Tags.
     - Xem lịch sử các bài viết do chính mình tạo.
     - Quản lý hồ sơ cá nhân và đổi mật khẩu.
   - **Admin (Tài khoản từ appsettings.json):** 
     - Quản lý tài khoản người dùng (`SystemAccount`) với Popup Modal CRUD.
     - Báo cáo thống kê số lượng bài viết theo khoảng thời gian (`StartDate` đến `EndDate`), sắp xếp giảm dần theo ngày tạo.

---

## 🚀 Hướng dẫn chạy dự án
1. Chạy script SQL `FUNewsManagement.sql` trên SQL Server Management Studio (SSMS).
2. Kiểm tra chuỗi kết nối trong `NguyenVanDiepMVC/appsettings.json`.
3. Mở Solution `NguyenVanDiep_SE19C.NET_A01.sln` trong Visual Studio hoặc chạy dòng lệnh:
```bash
dotnet run --project NguyenVanDiepMVC
```
4. Mở trình duyệt truy cập: `http://localhost:5269`

---

## 🔑 Tài khoản kiểm thử nhanh
- **Admin (appsettings.json):** `admin@FUNewsManagementSystem.org` / `@@abc123@@`
- **Staff (Database):** `IsabellaDavid@FUNewsManagement.org` / `@1`
- **Lecturer (Database):** `EmmaWilliam@FUNewsManagement.org` / `@1`
