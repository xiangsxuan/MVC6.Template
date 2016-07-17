using MvcTemplate.Components.Alerts;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Alerts
{
    public class AlertsContainerTests
    {
        private AlertsContainer container;

        public AlertsContainerTests()
        {
            container = new AlertsContainer();
        }

        #region Add(AlertType type, String message)

        [Fact]
        public void Add_TypedMessage()
        {
            container.Add(AlertType.Success, "Message");

            Alert actual = container.Single();

            Assert.Equal(AlertType.Success, actual.Type);
            Assert.Equal("Message", actual.Message);
            Assert.Equal(4000, actual.Timeout);
        }

        #endregion

        #region Add(AlertType type, String message, Int32 timeout)

        [Fact]
        public void Add_FadingTypedMessage()
        {
            container.Add(AlertType.Danger, "Message", 20);

            Alert actual = container.Single();

            Assert.Equal(AlertType.Danger, actual.Type);
            Assert.Equal("Message", actual.Message);
            Assert.Equal(20, actual.Timeout);
        }

        #endregion

        #region AddError(String message)

        [Fact]
        public void AddError_Message()
        {
            container.AddError("Message");

            Alert actual = container.Single();

            Assert.Equal(AlertType.Danger, actual.Type);
            Assert.Equal("Message", actual.Message);
            Assert.Equal(0, actual.Timeout);
        }

        #endregion

        #region AddError(String message, Int32 timeout)

        [Fact]
        public void AddError_FadingMessage()
        {
            container.AddError("Message", 1);

            Alert actual = container.Single();

            Assert.Equal(AlertType.Danger, actual.Type);
            Assert.Equal("Message", actual.Message);
            Assert.Equal(1, actual.Timeout);
        }

        #endregion

        #region Merge(AlertsContainer alerts)

        [Fact]
        public void Merge_DoesNotMergeItself()
        {
            container.Add(new Alert());
            IEnumerable<Alert> alerts = container.ToArray();

            container.Merge(container);

            IEnumerable<Alert> actual = container;
            IEnumerable<Alert> expected = alerts;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Merge_Alerts()
        {
            AlertsContainer part = new AlertsContainer();
            container.AddError("FirstError");
            part.AddError("SecondError");

            IEnumerable<Alert> expected = container.Union(part);
            IEnumerable<Alert> actual = container;
            container.Merge(part);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
