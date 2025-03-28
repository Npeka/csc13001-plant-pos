package csc13001.plantpos.customer;

import java.math.BigDecimal;

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

    @NotBlank(message = "Name is required")
    @Column(name = "name", length = 256)
    private String name;

    @NotBlank(message = "Phone number is required")
    @Column(name = "phone", length = 20, unique = true)
    private String phone;

    @Column(name = "email", length = 256)
    private String email;

    @Column(name = "gender", length = 10)
    private Gender gender;

    // type text
    @Column(name = "address", columnDefinition = "TEXT")
    private String address;

    @Column(name = "loyalty_points")
    private int loyaltyPoints;

    @Column(name = "loyalty_card_type")
    private CustomerType loyaltyCardType;

    public Customer(String name, String phone, String email, Gender gender) {
        this.name = name;
        this.phone = phone;
        this.gender = gender;
        this.email = email;
        this.loyaltyPoints = 0;
        this.loyaltyCardType = CustomerType.None;
    }

    public Customer(Long customerId, String name, String phone, String email, Gender gender) {
        this.customerId = customerId;
        this.name = name;
        this.phone = phone;
        this.gender = gender;
        this.email = email;
        this.loyaltyPoints = 0;
        this.loyaltyCardType = CustomerType.None;
    }

    public void addLoyaltyPointsBySpending(BigDecimal spending) {
        if (spending == null || spending.compareTo(BigDecimal.ZERO) <= 0) {
            throw new IllegalArgumentException("Spending must be greater than zero.");
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
