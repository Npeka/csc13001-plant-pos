package csc13001.plantpos.staff.dtos;

import java.math.BigDecimal;

import csc13001.plantpos.user.User;
import csc13001.plantpos.utils.http.JsonModel;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@EqualsAndHashCode(callSuper = true)
public class StaffDTO extends JsonModel {
    User user;
    int totalOrders;
    BigDecimal totalRevenue;
}
