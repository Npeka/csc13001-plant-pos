package csc13001.plantpos.statistic.dtos;

import csc13001.plantpos.user.User;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

@Data
@NoArgsConstructor
public class StaffStatisticsDTO {
    private User staff;
    private long totalOrders;
    private BigDecimal totalRevenue;

    public StaffStatisticsDTO(User staff, long totalOrders, BigDecimal totalRevenue) {
        this.staff = staff;
        this.totalOrders = totalOrders;
        this.totalRevenue = totalRevenue;
    }
}