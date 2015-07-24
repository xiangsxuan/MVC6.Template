using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MvcTemplate.Components.Mail
{
    public class SmtpMailClient : IMailClient
    {
        private String Sender { get; }
        private SmtpClient Client { get; }
        private Boolean Disposed { get; set; }

        public SmtpMailClient(String host, Int32 port, String sender, String password)
        {
            Sender = sender;
            Client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(sender, password),
                EnableSsl = true
            };
        }

        public Task SendAsync(String email, String subject, String body)
        {
            MailMessage mail = new MailMessage(Sender, email, subject, body);
            mail.SubjectEncoding = Encoding.UTF8;
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;

            return Client.SendMailAsync(mail);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (Disposed) return;

            Client.Dispose();

            Disposed = true;
        }
    }
}
