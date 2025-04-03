package csc13001.plantpos.customer;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.Date;

import com.fasterxml.jackson.annotation.JsonFormat;

import csc13001.plantpos.user.enums.Gender;
import jakarta.persistence.*;
import jakarta.validation.constraints.NotBlank;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "customers")
public class Customer {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "customer_id")
    private Long customerId;

    @NotBlank(message = "Tên khách hàng là bắt buộc")
    @Column(name = "name", length = 256)
    private String name;

    @NotBlank(message = "Số điện thoại là bắt buộc")
    @Column(name = "phone", length = 20, unique = true)
    private String phone;

    @Column(name = "email", length = 256)
    private String email;

    @Column(name = "gender", length = 10)
    private Gender gender;

    @Column(name = "address", columnDefinition = "TEXT")
    private String address;

    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "yyyy-MM-dd")
    @Column(name = "birth_date")
    private Date birthDate;

    @Column(name = "loyalty_points")
    private int loyaltyPoints;

    @Column(name = "loyalty_card_type")
    private CustomerType loyaltyCardType;

    @Builder.Default
    @Column(name = "created_at")
    @JsonFormat(shape = JsonFormat.Shape.STRING)
    private LocalDateTime createdAt = LocalDateTime.now();

    public void addLoyaltyPointsBySpending(BigDecimal spending) {
        if (spending == null || spending.compareTo(BigDecimal.ZERO) <= 0) {
            throw new IllegalArgumentException("Số tiền chi tiêu phải lớn hơn 0.");
        }
        loyaltyPoints += spending.divide(BigDecimal.valueOf(1000)).intValue();
        this.loyaltyCardType = determineCustomerType(this.loyaltyPoints);
    }

    private CustomerType determineCustomerType(int points) {
        if (points >= 45_000)
            return CustomerType.Platinum;
        if (points >= 15_000)
            return CustomerType.Gold;
        if (points >= 5_000)
            return CustomerType.Silver;
        if (points >= 1_000)
            return CustomerType.Bronze;
        return CustomerType.None;
    }
}
