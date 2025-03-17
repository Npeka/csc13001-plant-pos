using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace csc13001_plant_pos.Services
{
    public class MailService
    {
        private readonly string _smtpHost = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpEmail = "seimeicc@gmail.com";
        private readonly string _smtpPassword = "ctfp eowe ngzu vtma";

        public async Task SendOTPAsync(string receiverEmail, string otp)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Plant POS", _smtpEmail));
            message.To.Add(new MailboxAddress("User", receiverEmail));
            message.Subject = "Your OTP Code";
            message.Body = new TextPart("plain") { Text = $"Your OTP code is: {otp}" };

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpHost, _smtpPort, false);
                    await client.AuthenticateAsync(_smtpEmail, _smtpPassword);
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
}
