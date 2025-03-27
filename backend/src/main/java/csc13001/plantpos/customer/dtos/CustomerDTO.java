package csc13001.plantpos.customer.dtos;

import csc13001.plantpos.customer.Customer;
import lombok.Data;
import lombok.EqualsAndHashCode;

import java.math.BigDecimal;

import com.fasterxml.jackson.annotation.JsonAutoDetect;
import com.fasterxml.jackson.annotation.JsonInclude;

@Data
@EqualsAndHashCode(callSuper = true)
@JsonAutoDetect(fieldVisibility = JsonAutoDetect.Visibility.ANY)
@JsonInclude(JsonInclude.Include.NON_NULL)
public class CustomerDTO extends Customer {
    int totalOrders;
    BigDecimal totalSpent;

    public CustomerDTO(Customer customer) {
        super(
                customer.getCustomerId(),
                customer.getName(),
                customer.getPhone(),
                customer.getEmail(),
                customer.getGender(),
                customer.getLoyaltyPoints(),
                customer.getLoyaltyCardType());
        this.totalOrders = 0;
        this.totalSpent = BigDecimal.ZERO;
    }

    public CustomerDTO(Customer customer, int totalOrders, BigDecimal totalSpent) {
        super(
                customer.getCustomerId(),
                customer.getName(),
                customer.getPhone(),
                customer.getEmail(),
                customer.getGender(),
                customer.getLoyaltyPoints(),
                customer.getLoyaltyCardType());
        this.totalOrders = totalOrders;
        this.totalSpent = totalSpent;
    }
}
