package csc13001.plantpos.discount.models;

import jakarta.persistence.Entity;
import jakarta.persistence.Table;

import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Column;

import jakarta.persistence.Temporal;
import jakarta.persistence.TemporalType;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;
import java.util.Date;
import csc13001.plantpos.customer.Customer;
import csc13001.plantpos.discount.DiscountProgram;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "discount_usage")
public class DiscountUsage {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    @JoinColumn(name = "customer_id", nullable = false)
    private Customer customer;

    @ManyToOne
    @JoinColumn(name = "discount_id", nullable = false)
    private DiscountProgram discountProgram;

    @Column(name = "used_at", nullable = false)
    @Temporal(TemporalType.TIMESTAMP)
    private Date usedAt;
}
