# Hệ Thống Quản Lý Dự Án WinForms

## 1. Tổng quan dự án
Đây là dự án C# WinForms quản lý dự án, được xây dựng theo hướng lập trình hướng đối tượng, có lưu trữ dữ liệu bằng file JSON và tổ chức theo mô hình phân tầng đơn giản gồm Form, Controller, Service, Data, Model.

Luồng sử dụng chính đã được tích hợp trên cùng một màn hình dashboard:
- Tạo dự án
- Xem danh sách dự án
- Cập nhật trạng thái dự án
- Tạo task và giao nhân viên
- Xem task theo dự án
- Xóa dự án

## 2. Công nghệ sử dụng
- Ngôn ngữ: C#
- Framework: .NET 8 Windows Desktop (net8.0-windows)
- Giao diện: Windows Forms
- Serialize/Deserialize: System.Text.Json
- Lưu trữ: data.json trong thư mục output

## 3. Tính năng đã triển khai
### 3.1 Quản lý dự án
- Tạo dự án mới
- Xem danh sách dự án
- Xóa dự án
- Cập nhật trạng thái dự án
- Gán trước danh sách nhân viên tham gia khi tạo dự án

### 3.2 Quản lý task
- Tạo task cho dự án được chọn
- Giao task cho nhân viên
- Chọn trạng thái task
- Xem danh sách task của dự án

### 3.3 Quản lý nhân sự
- Mô hình kế thừa: AbsPerson -> Employee -> ProjectLeader
- Danh sách nhân viên được seed sẵn để demo nhanh

## 4. Cấu trúc thư mục
```
D:\oop_pro
|-- ProjectManagementSystem.sln
|-- README.md
`-- ProjectManagementSystem
    |-- Program.cs
    |-- ProjectManagementSystem.csproj
    |-- Models
    |   |-- AbsPerson.cs
    |   |-- Employee.cs
    |   |-- ProjectLeader.cs
    |   |-- EnumStatus.cs
    |   |-- Project.cs
    |   `-- TaskItem.cs
    |-- Builders
    |   `-- ProjectBuilder.cs
    |-- Services
    |   |-- IProjectService.cs
    |   `-- ProjectService.cs
    |-- Data
    |   |-- SystemContext.cs
    |   `-- DataStorage.cs
    |-- Controllers
    |   `-- ProjectController.cs
    `-- Forms
        |-- MainForm.cs
        |-- ProjectListForm.cs
        |-- CreateTaskForm.cs
        `-- CreateProjectForm.cs
```

## 5. Phân tích OOP chi tiết
## 5.1 Encapsulation (đóng gói)
Đóng gói được áp dụng xuyên suốt ở tầng Model bằng cách dùng private field và public property để kiểm soát dữ liệu.

Ví dụ:
- AbsPerson có các field riêng _id, _name, _age, _email và property Id, Name, Age, Email.
- Project có các field riêng cho thông tin dự án, leader, employees, tasks.
- TaskItem có field riêng cho mã task, tiêu đề, mô tả, trạng thái, người được giao.

Lợi ích:
- Không cho phép truy cập trực tiếp trạng thái nội bộ.
- Có thể chuẩn hóa dữ liệu ngay tại setter (ví dụ null -> string.Empty).

## 5.2 Abstraction (trừu tượng)
Trừu tượng được thể hiện qua interface và lớp abstract:

- IProjectService định nghĩa hợp đồng nghiệp vụ (tạo dự án, xóa dự án, cập nhật status, tạo task, lấy dữ liệu).
- ProjectService hiện thực chi tiết nghiệp vụ.
- AbsPerson là lớp abstract định nghĩa hành vi chung và method trừu tượng GetRole().

Lợi ích:
- Tách phần định nghĩa và phần hiện thực.
- Form/Controller làm việc với contract, giảm phụ thuộc chặt.

## 5.3 Inheritance (kế thừa)
Chuỗi kế thừa nhân sự:

- AbsPerson (gốc)
- Employee kế thừa AbsPerson
- ProjectLeader kế thừa Employee

Lợi ích:
- Tái sử dụng thuộc tính và hành vi chung.
- Dễ mở rộng thêm vai trò nhân sự mới về sau.

## 5.4 Polymorphism (đa hình)
Đa hình được thể hiện qua override phương thức và property:

- GetRole() được override giữa Employee và ProjectLeader.
- Property Role được override ở ProjectLeader để trả về vai trò leader.

Khi UI hiển thị tên + vai trò, cùng gọi GetRole() nhưng kết quả phụ thuộc kiểu thực tế của object.

## 6. Trách nhiệm từng class (class làm gì)
## 6.1 Nhóm Models
### AbsPerson
- Vai trò: lớp nền trừu tượng cho các loại người dùng.
- Chứa: Id, Name, Age, Email.
- Định nghĩa: GetRole() để lớp con hiện thực.

### Employee
- Vai trò: đại diện nhân viên thường.
- Kế thừa: AbsPerson.
- Thêm: Role (mặc định Employee), ToString() để hiển thị combo/list.

### ProjectLeader
- Vai trò: đại diện trưởng dự án.
- Kế thừa: Employee.
- Ghi đè: Role và GetRole() để phân biệt leader.

### EnumStatus
- Vai trò: enum trạng thái dùng chung cho dự án và task.
- Giá trị: Pending, OnGoing, Completed, Abandoned.

### TaskItem
- Vai trò: thực thể công việc thuộc dự án.
- Chứa: TaskId, Title, Description, Status, Assignee.

### Project
- Vai trò: aggregate root trong phạm vi nghiệp vụ quản lý dự án.
- Chứa:
  - Thông tin dự án: ProjectId, ProjectName, Description, StartDate, EndDate, Status.
  - Nhân sự: Leader, Employees.
  - Công việc: Tasks.

## 6.2 Nhóm Builders
### ProjectBuilder
- Vai trò: xây dựng object Project theo từng bước.
- Chức năng:
  - SetName, SetDescription, SetStartDate, SetEndDate, SetStatus, SetLeader, SetEmployees.
  - Build trả về Project hoàn chỉnh.
- Ghi chú:
  - Có xử lý chống trùng nhân viên.
  - Đảm bảo leader nằm trong danh sách nhân viên dự án.

## 6.3 Nhóm Data
### SystemContext (Singleton)
- Vai trò: trạng thái in-memory toàn ứng dụng.
- Quản lý:
  - Danh sách Project hiện tại.
  - Danh sách Employee seed sẵn.
  - CurrentProject.
- Chức năng chính:
  - AddProject, RemoveProjectById, ListProjects, SetProjects, GetEmployees.

### DataStorage (Singleton)
- Vai trò: làm việc với file data.json.
- Chức năng:
  - SaveData serialize danh sách project vào file.
  - LoadData deserialize từ file lên SystemContext.
- Có xử lý fallback khi file thiếu/rỗng/lỗi dữ liệu.

## 6.4 Nhóm Services
### IProjectService
- Vai trò: hợp đồng nghiệp vụ cho module dự án.
- Khai báo các hàm:
  - Tạo/xóa dự án
  - Cập nhật trạng thái dự án
  - Tạo task và giao nhân viên
  - Lấy danh sách project/task/status/employee

### ProjectService
- Vai trò: hiện thực toàn bộ nghiệp vụ.
- Trách nhiệm:
  - Validate dữ liệu đầu vào.
  - Sinh mã ProjectId và TaskId.
  - Chuẩn hóa danh sách nhân viên dự án.
  - Gọi DataStorage để lưu dữ liệu sau thao tác thay đổi.

## 6.5 Nhóm Controllers
### ProjectController
- Vai trò: cầu nối giữa Form và Service.
- Trách nhiệm:
  - Nhận input từ UI.
  - Gọi ProjectService theo đúng use case.
  - Trả kết quả và message về UI.

## 6.6 Nhóm Forms
### MainForm
- Vai trò: màn hình vào ứng dụng.
- Trách nhiệm:
  - Điều hướng người dùng vào dashboard quản lý dự án.

### ProjectListForm
- Vai trò: dashboard chính, nơi diễn ra phần lớn thao tác người dùng.
- Trách nhiệm:
  - Tạo dự án ngay trên cùng màn hình.
  - Gán trước nhân viên tham gia dự án.
  - Hiển thị danh sách dự án.
  - Cập nhật trạng thái dự án.
  - Mở form tạo task.
  - Hiển thị danh sách task theo dự án được chọn.
  - Xóa dự án.

### CreateTaskForm
- Vai trò: form chi tiết tạo task.
- Trách nhiệm:
  - Nhập tiêu đề, mô tả, trạng thái task.
  - Chọn assignee và tạo task.

### CreateProjectForm
- Vai trò: form tạo dự án độc lập cũ.
- Trạng thái hiện tại:
  - Vẫn tồn tại để tương thích.
  - Không còn là hành trình chính vì đã tích hợp vào dashboard.

## 6.7 Entry Point
### Program
- Vai trò: điểm bắt đầu ứng dụng.
- Trách nhiệm:
  - Khởi tạo WinForms app.
  - Load dữ liệu khi startup.
  - Save dữ liệu khi thoát app.
  - Chạy self-test khi truyền tham số --selftest.

## 7. Quy tắc nghiệp vụ và validation
- Bắt buộc nhập:
  - ProjectName, Description, Leader
  - Task Title, Task Description, Assignee
- Ràng buộc ngày:
  - StartDate phải nhỏ hơn hoặc bằng EndDate
- Dữ liệu nhân sự dự án:
  - Không trùng employee
  - Luôn có leader trong danh sách nhân viên tham gia

## 8. Mẫu thiết kế sử dụng
- Singleton:
  - SystemContext
  - DataStorage
- Builder:
  - ProjectBuilder

Không dùng thêm các mẫu thiết kế khác.

## 9. Lưu trữ dữ liệu
## 9.1 File dữ liệu
- Tên file: data.json
- Vị trí thường gặp:
  - ProjectManagementSystem/bin/Debug/net8.0-windows/data.json

## 9.2 Cơ chế lưu tải
- LoadData khi khởi động app.
- SaveData sau thao tác create/update/delete và khi app đóng.
- Nếu file lỗi hoặc không tồn tại, hệ thống fallback về danh sách rỗng.

## 10. Hướng dẫn chạy dự án
Từ thư mục gốc D:\oop_pro:

Build:
dotnet build .\ProjectManagementSystem.sln

Run:
dotnet run --project .\ProjectManagementSystem\ProjectManagementSystem.csproj

## 11. Chạy tự kiểm thử (self-test)
Lệnh:
dotnet run --project .\ProjectManagementSystem\ProjectManagementSystem.csproj -- --selftest

File kết quả:
ProjectManagementSystem/bin/Debug/net8.0-windows/selftest-result.txt

Self-test đang bao phủ:
- Tạo dự án
- Kiểm tra gán trước nhân viên
- Cập nhật trạng thái dự án
- Tạo task và giao nhân viên
- Chặn ngày không hợp lệ
- Lưu và nạp lại dữ liệu
- Xóa dự án

## 12. Luồng sử dụng đề xuất cho người dùng
1. Vào dashboard từ màn hình chính.
2. Tạo dự án tại khối Create New Project.
3. Chọn leader và tick nhân viên tham gia.
4. Tạo dự án.
5. Chọn dự án trong bảng.
6. Cập nhật trạng thái dự án nếu cần.
7. Tạo task và giao nhân viên.
8. Theo dõi task bên dưới cùng màn hình.

## 13. Ràng buộc coding đã tuân thủ
- Không dùng lambda trong source do người viết.
- Không dùng LINQ trong source do người viết.
- Dùng vòng lặp tường minh cho các thao tác duyệt/lọc.

## 14. Gợi ý mở rộng
- Sửa/xóa task
- Đổi trạng thái task trực tiếp trên bảng task
- Tìm kiếm và lọc dự án theo trạng thái
- Màn hình CRUD nhân viên
- Xuất báo cáo
