using System;
using System.Threading.Tasks;

namespace Renting.Components.Mail
{
    public interface IMailClient
    {
        Task SendAsync(String email, String subject, String body);
    }
}
