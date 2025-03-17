package csc13001.plantpos.domain.models;

import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;

import jakarta.persistence.*;
import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

@Data
@NoArgsConstructor
@Entity
@EntityListeners(AuditingEntityListener.class)
@JsonInclude(JsonInclude.Include.NON_NULL)
@Table(name = "users")
public class User {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "user_id")
    private Long userId;

    @NotBlank(message = "Fullname is required")
    @Size(min = 5, max = 256, message = "Fullname must be between 5 and 256 characters")
    @Column(name = "fullname", length = 256)
    private String fullname;

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

    @NotBlank(message = "Password is required")
    @Size(min = 5, max = 255, message = "Password must be between 5 and 255 characters")
    @Column(name = "password", nullable = false)
    private String password;

    @JsonProperty("isAdmin")
    @NotNull(message = "isAdmin is required")
    @Column(name = "is_admin", nullable = false)
    private boolean isAdmin;

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
    }

    @JsonIgnore
    public String getRole() {
        return isAdmin ? "admin" : "staff";
    }
}
