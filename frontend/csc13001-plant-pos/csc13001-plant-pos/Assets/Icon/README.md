# MILESTONE 1: KARAOKEY - MÔN HỌC: LẬP TRÌNH WINDOWS - LỚP: CQ2022/31

### **Thông tin thành viên**

| MSSV     | Tên thành viên      |
| -------- | ------------------- |
| 22120222 | Võ Văn Nam          |
| 22120232 | Lê Nguyễn Hồng Ngọc |

## Các công việc thực hiện được trong Milestone 1

### Công việc 1: <u>Tính năng Đăng ký, đăng nhập</u> (1 giờ)

Chức năng đăng nhập ở mức cơ bản, nội dung mà nhóm thực hiện làm trong milestone này là:

- **Đăng nhập bằng tài khoản sử dụng email**: người dùng sẽ nhập email và mật khẩu để đăng nhập vào ứng dụng. Hỗ trợ thêm cho người dùng các chức năng như Remember Me và Forget Password.
- **Giao diện cho chức năng đăng nhập**: Giao diện này sẽ hiển thị giao diện đăng nhập, cho phép nhập email, mật khẩu, cho phép tùy chọn nhớ mật khẩu - Remember me và quên mật khẩu - Forgot passwword..

Ứng dụng đã có thể đăng nhập qua email và password, ghi nhớ mật khẩu remember me. Dữ liệu đầu vào đã được kiểm tra (định dạng email sai, dữ liệu trống, mật khẩu ngắn hơn 8 ký tự, v.v.).\
Ngoài ra, ứng dụng còn có tính năng tự động đăng nhập nếu đã sử dụng remember me từ trước. Các tính năng này đều cài đặt dựa trên giao diện của service của IAuthenticationService và AuthorizationActivationHandler, do đó, việc thay đổi cách hoạt đằng sau sẽ vô cùng dễ dàng. \
Hiện tại, ứng dụng đang lưu thông tin đăng nhập tại Local Settings. Sau này, với việc có database và thay đổi IDao service sang sử dụng API, nhóm có thể tính đến việc thay đổi authentication logic bằng JWTs
![alt text](README_img/image-1.png)

### Công việc 2: <u>Tính năng Giao diện khách hàng</u> (3 giờ)

Hoàn thiện giao diện và các tính năng cho giao diện của người dùng. Giao diện người dùng sẽ gồm có:

#### Trang chủ:

Trang chủ cho phép người hát có thể tìm kiếm và chọn bài hát dựa trên danh sách các bài hát đã được lưu trên hệ thống. Ngoài ra, người dùng còn có thể tìm kiếm bài hát, xem danh sách các bài đã được lựa chọn, hoặc ưu tiên một bài hát nào đó lên trước.
Trang Homepage này sẽ có những nội dung sau:

1. **Thanh menu**: Ở thanh này sẽ hiển thị những lựa chọn sau: Trang chủ, Youtube, Ca sĩ, Đã chọn, Tra cứu, Darkmode.
2. **Top 10 thịnh hành**: hiển thị danh sách top 10 bài hát thịnh hành nhất.
3. **Gần đây**: Danh sách các bài hát gần đây nhất.
4. **Ca sĩ**: Danh sách các ca sĩ gần đây nhất.
5. **Bài hát hiện tại**: Hiển thị và phát video karaoke hiện tại cho người dùng có thể sử dụng để nghe và hát.
6. **Thanh tìm kiếm**: Thanh tìm kiếm để tìm kiếm theo bài hát, ca sĩ.
7. **Thanh điều khiển media**: Giao diện cho phép người dùng có thể phát, phát lại, qua bài hoặc chọn dịch vụ. Ngoài ra người dùng còn có thể tăng giảm âm lượng cho phù hợp với nhu cầu sử dụng.

Hiện tại các giao diện Homepage đã hoàn thiện layout và màu sắc ở mức cơ bản, tuy vậy vẫn còn một số lỗi.

#### Youtube Search

Trang Youtube Search được sử dụng để tìm kiếm bài hát trên youtube mà không có trên hệ thống. Hiện tại, người dùng có thể tìm kiếm bài hát dựa trên API của Youtube và vào màn hình fullscreen để xem video. \
Hiện tại, do một số chính sách của Youtube nên một số video sẽ không xem được trên giao diện của app mà phải xem bên ngoài. Nhóm sẽ cố gắng chỉnh sửa lại trong milestone tiếp theo

#### Order Service

Trang Order Service để thực hiện gọi dịch vụ của quán. Các dịch vụ có thể là đồ ăn nhẹ như trái cây, hoa quả,... hoặc đồ uống như nước khoáng, bia, bò húc. Ngoài ra, còn có các loại dịch vụ khác như là thuê micro, hay thuê thêm giờ cho loại thuê phòng có thời hạn.

### Công việc 3: <u>Tính năng Giao diện admin</u> (1 giờ)

Hiện các tính năng của giao diện admin chưa hoàn thiện quá nhiều (khoảng 15% so với dự tính). Admin hiện tại có thể theo dõi danh sách các sản phẩm dịch vụ của một cơ sở mình điều hành. Admin cũng có thể thêm một dịch vụ mới vào trong danh sách.
![alt text](README_img/image-2.png)
_Giao diện xem các dịch vụ của admin_
![alt text](README_img/image-3.png)
_Giao diện thêm mới một dịch vụ_

Các tính năng còn lại thuộc giao diện Admin gồm:

- Hoàn thiện tính năng cho Facility Management, cho phép quản lí về số lượng phòng, giá phòng ở chi nhánh đó.
- Phân quyền, cho phép một nhân viên (hiện tại chỉ có thu ngân) tham gia vào làm việc tại một chi nhánh.
- Dashboard cho phép quan sát doanh thu, số lượng đơn hàng, số lượng phòng đang được sử dụng

### Công việc 4: <u>Tính năng Thiết lập khung giờ đặc biệt</u> (0.5 giờ)

Tính năng thiết lập khung giờ đặc biệt cho phép thiết lập một khoảng thời gian mà ở đó, dịch vụ sẽ được tính ở một giá tiền khác. **Ví dụ**, ở các ngày cuối tuần, giá phòng có thể cao hơn so với các ngày khác.\
Hiện tại giao diện để thiết lập khung giờ đặc biệt đã tương đối đầy đủ (hình ảnh phía trên). Tuy vậy, để sử dụng tính năng này cần thiết lập tính năng ước tính giá tiền và thanh toán cho một phòng (mà hiện tại nhóm chưa cài đặt các tính năng này). Các tính năng này sẽ bắt đầu được phát triển sau khi giao diện của thu ngân được bắt đầu đi vào quá trình cài đặt.

### Công việc 5: <u>Các thiết lập về database cơ bản và flow chương trình</u> (0.5 giờ)

Nhóm đã hình thành sơ đồ ER cho database của hệ thống cũng như thiết lập flow chương trình dành cho người sử dụng ứng dụng. \
**Flow chương trình** \
Khi bắt đầu vào ứng dụng, người dùng sẽ phải đăng nhập vào tài khoản. Tài khoản này sẽ được đăng ký lên server của ứng dụng như một user. User này vừa có thể là admin để quản lý các chi nhánh khác nhau của một quán Karaoke, vừa có thể là nhân viên làm việc tại các quán ấy.\
Khi vào ứng dụng, sẽ có 2 phần, 1 phần hiện các chi nhánh mà User quản lý, nhánh còn lại là các chi nhánh mà mình làm việc. \
User có thể request để tạo chi nhánh mới mà mình quản lý hoặc request để tham gia vào một chi nhánh cụ thể nào đó để làm nhân viên. Tương ứng với từng phần mà khi người dùng bấm vào sẽ vào giao diện tương ứng với vai trò của tài khoản đó tại chi nhánh hiện tại. \
**ER diagram**
![ER diagram](README_img/Karaokey_DB_ERDiagram.png)

## DESIGN PATTERNS/ ARCHITECTURE ĐƯỢC SỬ DỤNG TRONG MILESTONE 1

- **Về Architecture**: Nhóm em đang sử dụng mô hình 3 Layers với tầng UI được đảm nhiệm bởi các Page và ViewModel. Các dịch vụ truy xuất đến cơ sở dữ liệu sẽ thông qua các service và trao đổi thông tin bằng các Models. Như vậy, models đóng vai trò trung gian, là một Data transfer object để ViewModel có thể sử dụng. Cấu trúc folder của nhóm như sau:

```plaintext
KaraoKey
├── Activation/
├── Assets/
├── Behaviors/
├── Contracts/
├── Helpers/
├── Models/
│   ├── Facility.cs
│   ├── LocalSettingsOptions.cs
│   ├── ServiceItem.cs
│   ├── Singer.cs
│   ├── Song.cs
│   └── Video.cs
├── Properties/
├── Services/
│   ├── DataAccess
│   │   └── MockDao.cs
│   ├── ... (Các Service khác của ứng dụng)
├── Strings/en-us/
├── Styles/
├── ViewModels/
│   ├── Admin/
│   ├── Auth/
│   │   └── LoginShellViewModel.cs
│   ├── Cashier/
│   ├── Customer/
│   ├── NavigateCenter/
│   ├── MainViewModel.cs
│   ├──SettingsViewModel.cs
│   ├── FacilityViewModel.cs
│   └── ... (Các ViewModel khác của ứng dụng)
├── Views/
│   ├── AdminView/
│   ├── AuthView/
│   ├── CustomerView/
│   ├── ShellPages/
│   │   ├── LoginShellPage.xaml
│   │   ├── LoginShellPage.xaml.cs
│   │   ├── UserShellPage.xaml
│   │   ├── UserShellPage.xaml.cs
│   │   └── ... (Các ShellPage khác)
│   ├── MainPage.xaml
│   ├── MainPage.xaml.cs
│   ├── SettingsPage.xaml
│   ├── SettingsPage.xaml.cs
│   └── ...
├── App.xaml
├── App.xaml.cs
└── ... (Các file sln, readme và các file khác cần thiết)
```

- **Về UI design patterns** nhóm có sử dụng mô hình MVVM với View là các Page có nhiệm vụ hiện các thông tin lên giao diện và truyền các hành động của người dùng xuống cho ViewModel để xử lý. ViewModel sẽ thực hiện trung gian, gọi các service để xử lý các tác vụ của người dùng và qua data binding, hiện lên trên giao diện.
- **Về design patterns** nhóm có sử dụng Dependency Injection và Singleton với các dịch vụ đều được đăng ký trước ở file app và được gọi xuyên suốt ứng dụng thông qua GetService

## ADVANCED TOPIC CỦA NHÓM TRONG MILESTONE 1

Các topic nâng cao mà nhóm đã sử dụng trong milestone này:

- Navigation nâng cao: Vì ứng dụng có nhiều giao diện. Mặt khác, các hoạt động navigation được thực hiện thông qua các service, do đó, nhóm có cài đặt lại một số service cho navigation như WindowNavigationService hay FrameNavigationService
- Làm việc với API của youtube: Để tránh lưu quá nhiều dữ liệu trên database về các bài hát, một cách tiếp cận tốt hơn là sử dụng youtube nhúng trong ứng dụng. Do đó, nhóm đã tìm hiểu về cách sử dụng data API của Youtube, đồng thời sử dụng WebView2 để thực hiện nhúng Youtube video vào ứng dụng.

## TEAMWORK - GITFLOW TRONG MILESTONE 1

- Về Git: Nhóm sẽ có 2 source, 1 Source dành cho ứng dụng chính và Source dành cho Backend Service.
- Gitflow: nhóm sử dụng feature gitflow cho toàn bộ quá trình với các nhánh tính năng được phát triển trên nhánh feature vào sau đó được merge vào main thông qua pull request.
  - Hình ảnh minh chứng về hoạt động Git của nhóm:
    - ![image 1](https://drive.google.com/file/d/1Z_wD8bHFRTEfYS2DYod3ae_MXKSxIYH-/view?usp=sharing)

## QUALITY ASSURANCE TRONG MILESTONE 1

- **Quá trình duyệt mã nguồn** Quy trình push code được nhóm quy định như sau:

  1. **main**: Ở branch này là nơi chứa source chính sau khi được kiểm thử thành công và nộp bài sau mỗi milestone.
  2. **feature/xx**: Các thành viên sẽ chủ yếu code trên branch này và branch này sẽ được checkout từ main để tạo ra

- Một số hình ảnh minh chứng về hoạt động kiểm duyệt mã nguồn của nhóm:
  - ![alt text](README_img/image.png)
- **UI Test**: Ngoài ra nhóm kết hợp với việc viết UI test cũng như là mannual testing để đảm bảo ít lỗi xảy ra nhất. Các test case thầy có thể tìm thấy ở link [Google Sheets này](https://docs.google.com/spreadsheets/d/1sQBJqmsavbwg0YHBH4jiulvB83lMp6VuE9wcW4pF6T4/edit?usp=sharing)
