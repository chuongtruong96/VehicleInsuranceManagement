using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
namespace Project3.Models
{
    public interface IEmail
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
    public class Email : IEmail
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            //content mail
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("huy", "nqht123456789@gmail.com"));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new TextPart("html")  // dùng cái này để có thể đọc đc dạng html khi viết html trong controller

            {
                Text = body
            };
            //config
            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("nqht123456789@gmail.com", "hftr dolk cibg uwrv");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
