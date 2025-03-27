package csc13001.plantpos.staff.dtos;

import java.math.BigDecimal;

import com.fasterxml.jackson.annotation.JsonAutoDetect;
import com.fasterxml.jackson.annotation.JsonInclude;

import csc13001.plantpos.user.User;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@AllArgsConstructor
@JsonAutoDetect(fieldVisibility = JsonAutoDetect.Visibility.ANY)
@JsonInclude(JsonInclude.Include.NON_NULL)
@EqualsAndHashCode(callSuper = true)
public class StaffDTO extends User {
    int totalOrders;
    BigDecimal totalRevenue;

    public StaffDTO(User user, int totalOrders, BigDecimal totalRevenue) {
        super(user);
        this.totalOrders = totalOrders;
        this.totalRevenue = totalRevenue;
    }
}
