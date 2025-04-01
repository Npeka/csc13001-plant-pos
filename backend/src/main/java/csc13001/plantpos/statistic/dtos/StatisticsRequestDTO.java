package csc13001.plantpos.statistic.dtos;

import java.time.LocalDateTime;

import csc13001.plantpos.statistic.TimeType;
import lombok.Data;

@Data
public class StatisticsRequestDTO {
    private TimeType timeType;
    private LocalDateTime startDate;
    private LocalDateTime endDate;
}
