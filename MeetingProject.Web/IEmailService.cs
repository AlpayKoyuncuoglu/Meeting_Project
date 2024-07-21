using System.Net.Mail;
using System.Net;

namespace MeetingProject.Web
{

    public interface IEmailService
    {
        //Task SendEmailAsync(EmailMessage emailMessage);
        Task SendEmailAsync(string toEmail, string subject, string message);

    }

    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        //var smtpClient = new SmtpClient("smtp.example.com", 587) // SMTP sunucusu adresi ve portu
        //{
        //    Credentials = new NetworkCredential("username", "password"), // Kimlik doğrulama bilgileri
        //    EnableSsl = true // SSL/TLS kullanımı
        //};
        public EmailService()
        {
            // SmtpClient nesnesini oluşturun ve yapılandırın
            //_smtpClient = new SmtpClient("smtp.your-email-provider.com")
            //{
            //    Port = 587,
            //    Credentials = new NetworkCredential("your-email@example.com", "your-email-password"),
            //    EnableSsl = true,
            //};
            _smtpClient = new SmtpClient("smtp.example.com", 587) // SMTP sunucusu adresi ve portu
            {
                Credentials = new NetworkCredential("username", "password"), // Kimlik doğrulama bilgileri
                EnableSsl = true // SSL/TLS kullanımı
            };
        }
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("your-email@example.com"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);
            try
            {
                await _smtpClient.SendMailAsync(mailMessage);

            }
            catch (Exception e)
            {

                throw e;
            }
        }
        //public async Task SendEmailAsync(EmailMessage emailMessage)
        //{
        //    using (var client = new SmtpClient("smtp.example.com"))
        //    {
        //        var mailMessage = new MailMessage
        //        {
        //            From = new MailAddress("your-email@example.com"),
        //            Subject = emailMessage.Subject,
        //            Body = emailMessage.Content,
        //            IsBodyHtml = true
        //        };
        //        mailMessage.To.Add(emailMessage.To);

        //        client.Credentials = new NetworkCredential("your-email@example.com", "your-email-password");
        //        client.EnableSsl = true;

        //        await client.SendMailAsync(mailMessage);
        //    }
        //}
    }

    public class EmailMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }

}
