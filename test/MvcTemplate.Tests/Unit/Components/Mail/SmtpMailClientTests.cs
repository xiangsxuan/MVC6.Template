using MvcTemplate.Components.Mail;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mail
{
    public class SmtpMailClientTests
    {
        #region Dispose()

        [Fact]
        public void Dispose_MultipleTimes()
        {
            SmtpMailClient client = new SmtpMailClient(ConfigurationFactory.Create());

            client.Dispose();
            client.Dispose();
        }

        #endregion
    }
}
