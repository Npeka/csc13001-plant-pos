package csc13001.plantpos.domain.models;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
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

    @Column(name = "name", length = 256)
    private String name;

    @NotBlank(message = "Phone number is required")
    @Column(name = "phone", length = 20, unique = true)
    private String phone;

    @Column(name = "email", length = 256)
    private String email;

    @Column(name = "address")
    private String address;

    @Column(name = "loyalty_points")
    private int loyaltyPoints;

    @Column(name = "loyalty_card_type")
    private String loyaltyCardType;
}
