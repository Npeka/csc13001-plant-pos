using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace csc13001_plant_pos.Model
{
    public partial class User : ObservableObject
    {
        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("imageUrl")]
        public string? ImageUrl { get; set; }

        [JsonPropertyName("fullname")]
        public string Fullname { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("gender")]
        public string? Gender { get; set; }

        [JsonPropertyName("isAdmin")]
        public bool IsAdmin { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [ObservableProperty]
        private bool _isSelected;
        [JsonPropertyName("canManageDiscounts")]
        public bool CanManageDiscounts { get; set; }
        [JsonPropertyName("canManageInventory")]
        public bool CanManageInventory { get; set; }
        [JsonPropertyName("workLogs")]
        public List<WorkLog> WorkLogs { get; set; }
    }
}