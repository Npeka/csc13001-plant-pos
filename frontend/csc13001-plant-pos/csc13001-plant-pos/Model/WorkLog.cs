using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace csc13001_plant_pos.Model
{
    public class WorkLog
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("loginTime")]
        public string? LogInTime { get; set; }

        [JsonPropertyName("logoutTime")]
        public string LogOutTime { get; set; }

        [JsonPropertyName("workDuration")]
        public string WorkDuration { get; set; }
    }
}
