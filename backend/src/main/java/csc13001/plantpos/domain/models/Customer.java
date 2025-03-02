package csc13001.plantpos.domain.models;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@Entity
@Table(name = "customers")
public class Customer {

    @Id
    @Column(name = "customer_id")
    private Long customerId;

    @Column(name = "name", length = 256)
    private String name;

    @Column(name = "phone", length = 20)
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
