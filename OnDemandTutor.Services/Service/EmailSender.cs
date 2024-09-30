using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using OnDemandTutor.Contract.Services.Interface;
using System.Net.Mail;

namespace OnDemandTutor.Services.Service
{
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername = "newsexpressproject@gmail.com";
        private readonly string _smtpPassword = "mbibryjcyxzrygvy";

        public async Task SendOtpEmailAsync(string email, string otp)
        {
            var emailMessage = new MimeMessage
            {
                From = { new MailboxAddress("On Demand Tutor", "no-reply@gmail.com") },
                To = { new MailboxAddress("", email) },
                Subject = "Your OTP Code",
                Body = new TextPart("plain")
                {
                    Text = $"Your OTP code is {otp}"
                }
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
