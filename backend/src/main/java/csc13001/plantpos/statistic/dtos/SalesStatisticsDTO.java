package csc13001.plantpos.statistic.dtos;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.util.List;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class SalesStatisticsDTO {
    private BigDecimal revenue;
    private BigDecimal revenueGrowthRate;
    private BigDecimal profit;
    private BigDecimal profitGrowthRate;
    private long orderCount;
    private long orderCountGrowthRate;

    private BigDecimal growthRate;
    private List<TimeSeriesRevenue> timeSeriesRevenues;

    @Data
    @NoArgsConstructor
    @AllArgsConstructor
    public static class TimeSeriesRevenue {
        private String time;
        private BigDecimal revenue;
    }
}