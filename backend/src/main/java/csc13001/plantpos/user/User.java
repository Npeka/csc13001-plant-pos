package csc13001.plantpos.user;

import com.fasterxml.jackson.annotation.JsonFormat;
import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;
import csc13001.plantpos.user.enums.Gender;
import csc13001.plantpos.user.enums.WorkingStatus;
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

import org.springframework.data.jpa.domain.support.AuditingEntityListener;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "users")
@EntityListeners(AuditingEntityListener.class)
@JsonInclude(JsonInclude.Include.NON_NULL)
public class User {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "user_id")
    private Long userId;

    @NotBlank(message = "Fullname is required")
    @Size(min = 5, max = 256, message = "Fullname must be between 5 and 256 characters")
    @Column(name = "fullname", length = 256)
    private String fullname;

    @JsonProperty(access = JsonProperty.Access.WRITE_ONLY)
    @NotBlank(message = "Username is required")
    @Size(min = 5, max = 50, message = "Username must be between 5 and 50 characters")
    @Column(name = "username", nullable = false, unique = true)
    private String username;

    @Email(message = "Email is invalid")
    @Column(name = "email", nullable = true, unique = true)
    private String email;

    @Size(min = 10, max = 20, message = "Phone must be between 10 and 20 characters")
    @Column(name = "phone", length = 20)
    private String phone;

    @JsonProperty(access = JsonProperty.Access.WRITE_ONLY)
    @NotBlank(message = "Password is required")
    @Size(min = 5, max = 255, message = "Password must be between 5 and 255 characters")
    @Column(name = "password", nullable = false)
    private String password;

    @JsonProperty("isAdmin")
    @NotNull(message = "isAdmin is required")
    @Column(name = "is_admin", nullable = false)
    private boolean isAdmin;

    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "yyyy-MM-dd")
    @Column(name = "start_date", columnDefinition = "DATE")
    private LocalDate startDate;

    @Column(name = "status")
    private WorkingStatus status;

    @Enumerated(EnumType.STRING)
    @Column(name = "gender")
    private Gender gender;

    public User(String fullname, String username, String password) {
        this.fullname = fullname;
        this.username = username;
        this.password = password;
        this.isAdmin = false;
        this.startDate = LocalDate.now();
    }

    public User(String fullname, String username, String password, boolean isAdmin) {
        this.fullname = fullname;
        this.username = username;
        this.password = password;
        this.isAdmin = isAdmin;
        this.startDate = LocalDate.now();
        this.status = WorkingStatus.Working;
    }

    @JsonIgnore
    public String getRole() {
        return isAdmin ? "admin" : "staff";
    }
}
