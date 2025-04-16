using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.DTO.Message;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Utils;

namespace csc13001_plant_pos.Service
{
    public interface IMessageService
    {
        Task<ApiResponse<Message>?> SendMessageAsync(CreateMessageDto messageDto);
        Task<ApiResponse<List<Message>>?> GetMessagesByUserIdAsync(int userId);
    }

    public class MessageService : IMessageService
    {
        private readonly HttpClient _httpClient;

        public MessageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<Message>?> SendMessageAsync(CreateMessageDto messageDto)
        {
            try
            {
                var content = JsonUtils.ToJsonContent(messageDto);
                var response = await _httpClient.PostAsync("messages", content);
                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonUtils.Deserialize<ApiResponse<Message>>(json);
                return apiResponse;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sending message: {ex.Message}");
                return null;
            }
        }

        public async Task<ApiResponse<List<Message>>?> GetMessagesByUserIdAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"messages/user/{userId}");
                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonUtils.Deserialize<ApiResponse<List<Message>>>(json);
                return apiResponse;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting messages for user {userId}: {ex.Message}");
                return null;
            }
        }
    }
}