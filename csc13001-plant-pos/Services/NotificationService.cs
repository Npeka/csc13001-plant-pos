using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csc13001_plant_pos.Core.Contracts.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace csc13001_plant_pos.Services;
public class SmtpSettings
{
    public string Host { get; set; } = "";
    public int Port { get; set; } = 587;
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}

public class NotificationService : INotificationService
{
    private readonly SmtpSettings _smtpSettings;

    public NotificationService(IOptions<SmtpSettings> smtpSettings)
    {
        this._smtpSettings = smtpSettings.Value;
    }

    public async Task SendOTP(string receiverEmail, string otp)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Plant POS", _smtpSettings.Email));
        message.To.Add(new MailboxAddress("User", receiverEmail));
        message.Subject = "Your OTP Code";
        message.Body = new TextPart("plain") { Text = $"Your OTP code is: {otp}" };

        try
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, false);
                await client.AuthenticateAsync(_smtpSettings.Email, _smtpSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                Console.WriteLine("✅ Email sent successfully!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Failed to send email: {ex.Message}");
        }
    }
}
