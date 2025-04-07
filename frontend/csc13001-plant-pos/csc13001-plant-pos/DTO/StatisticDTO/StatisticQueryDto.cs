﻿using System;
using System.Text.Json.Serialization;

namespace csc13001_plant_pos.DTO.StatisticDTO
{
    public class StatisticQueryDto
    {
        [JsonPropertyName("timeType")]
        public string TimeType { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }
        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }
    }
}