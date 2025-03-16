package csc13001.plantpos.domain.models;

import com.fasterxml.jackson.annotation.JsonInclude;
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

    @NotBlank(message = "Username is required")
    @Size(min = 5, max = 50, message = "Username must be between 5 and 50 characters")
    @Column(name = "username", nullable = false, unique = true)
    private String username;

    @Email(message = "Email is invalid")
    @Column(name = "email", nullable = true, unique = true)
    private String email;

    @NotBlank(message = "Password is required")
    @Size(min = 5, max = 255, message = "Password must be between 5 and 255 characters")
    @Column(name = "password", nullable = false)
    private String password;

    @NotNull(message = "isAdmin is required")
    @Column(name = "is_admin", nullable = false)
    private boolean isAdmin;

    public User(String username, String password) {
        this.username = username;
        this.password = password;
        this.isAdmin = false;
    }

    public User(String username, String password, String email, boolean isAdmin) {
        this.username = username;
        this.password = password;
        this.email = email;
        this.isAdmin = isAdmin;
    }

    public String getRole() {
        return isAdmin ? "admin" : "staff";
    }
}
