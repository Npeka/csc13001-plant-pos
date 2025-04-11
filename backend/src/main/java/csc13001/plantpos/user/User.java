package csc13001.plantpos.user;

import com.fasterxml.jackson.annotation.JsonFormat;
import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.annotation.JsonProperty;
import csc13001.plantpos.user.enums.Gender;
import csc13001.plantpos.user.enums.WorkingStatus;
import csc13001.plantpos.user.models.WorkLog;
import jakarta.persistence.*;
import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.time.LocalDate;
import java.util.List;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "users")
public class User {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "user_id")
    private Long userId;

    @Column(name = "image_url")
    private String imageUrl;

    @NotBlank(message = "Họ tên là bắt buộc")
    @Size(min = 5, max = 256, message = "Họ tên phải từ 5 đến 256 ký tự")
    @Column(name = "fullname", length = 256)
    private String fullname;

    @JsonProperty(access = JsonProperty.Access.WRITE_ONLY)
    @NotBlank(message = "Tên đăng nhập là bắt buộc")
    @Size(min = 5, max = 50, message = "Tên đăng nhập phải từ 5 đến 50 ký tự")
    @Column(name = "username", nullable = false, unique = true)
    private String username;

    @Email(message = "Email không hợp lệ")
    @Column(name = "email", nullable = true, unique = true)
    private String email;

    @Size(min = 10, max = 20, message = "Số điện thoại phải từ 10 đến 20 ký tự")
    @Column(name = "phone", length = 20)
    private String phone;

    @JsonProperty(access = JsonProperty.Access.WRITE_ONLY)
    @NotBlank(message = "Mật khẩu là bắt buộc")
    @Size(min = 5, max = 255, message = "Mật khẩu phải từ 5 đến 255 ký tự")
    @Column(name = "password", nullable = false)
    private String password;

    @Column(name = "gender")
    private Gender gender;

    @JsonFormat(shape = JsonFormat.Shape.STRING)
    @Column(name = "start_date", columnDefinition = "DATE")
    private LocalDate startDate;

    @Column(name = "status")
    private WorkingStatus status;

    @JsonProperty("isAdmin")
    @NotNull(message = "isAdmin là bắt buộc")
    @Column(name = "is_admin", nullable = false)
    private boolean isAdmin;

    @OneToMany
    private List<WorkLog> workLogs;

    public User(String fullname, String username, String password) {
        this.fullname = fullname;
        this.username = username;
        this.password = password;
        this.isAdmin = false;
    }

    public User(String fullname, String username, String password, boolean isAdmin) {
        this.fullname = fullname;
        this.username = username;
        this.password = password;
        this.isAdmin = isAdmin;
        this.status = WorkingStatus.Working;
    }

    public User(User other) {
        this.userId = other.userId;
        this.fullname = other.fullname;
        this.username = other.username;
        this.email = other.email;
        this.phone = other.phone;
        this.password = other.password;
        this.isAdmin = other.isAdmin;
        this.startDate = other.startDate;
        this.status = other.status;
        this.gender = other.gender;
    }

    @JsonIgnore
    public String getRole() {
        return isAdmin ? "admin" : "staff";
    }
}
