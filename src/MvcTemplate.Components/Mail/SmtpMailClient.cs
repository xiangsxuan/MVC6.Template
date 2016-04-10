using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MvcTemplate.Components.Mail
{
    public class SmtpMailClient : IMailClient
    {
        private SmtpClient Client { get; }
        private Boolean Disposed { get; set; }
        private IConfigurationSection Config { get; }

        public SmtpMailClient(IConfiguration config)
        {
            Config = config.GetSection("Mail");
            Client = new SmtpClient(Config["Host"], Int32.Parse(Config["Port"]))
            {
                Credentials = new NetworkCredential(Config["Sender"], Config["Password"]),
                EnableSsl = Boolean.Parse(Config["EnableSsl"])
            };
        }

        public Task SendAsync(String email, String subject, String body)
        {
            MailMessage mail = new MailMessage(Config["Sender"], email, subject, body);
            mail.SubjectEncoding = Encoding.UTF8;
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;

            return Client.SendMailAsync(mail);
        }

        public void Dispose()
        {
            if (Disposed) return;

            Client.Dispose();

            Disposed = true;
        }
    }
}
