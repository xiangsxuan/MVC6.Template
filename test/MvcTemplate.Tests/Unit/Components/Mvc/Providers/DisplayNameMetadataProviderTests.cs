using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.ModelBinding.Metadata;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class DisplayNameMetadataProviderTests
    {
        #region CreateModelMetadata(DefaultMetadataDetails entry)

        [Fact]
        public void CreateModelMetadata_SetsDisplayName()
        {
            DefaultMetadataDetails metaDetails = new DefaultMetadataDetails(
                ModelMetadataIdentity.ForProperty(typeof(String), "Title", typeof(RoleView)), ModelAttributes.GetAttributesForType(typeof(RoleView)));
            ICompositeMetadataDetailsProvider details = Substitute.For<ICompositeMetadataDetailsProvider>();
            DisplayNameMetadataProviderProxy provider = new DisplayNameMetadataProviderProxy(details);

            String expected = ResourceProvider.GetPropertyTitle(typeof(RoleView), "Title");
            String actual = provider.BaseCreateModelMetadata(metaDetails).DisplayName;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateModelMetadata_NullContainerType_DoesNotSetDisplayName()
        {
            DefaultMetadataDetails metaDetails = new DefaultMetadataDetails(
                new ModelMetadataIdentity(), ModelAttributes.GetAttributesForType(typeof(RoleView)));
            ICompositeMetadataDetailsProvider details = Substitute.For<ICompositeMetadataDetailsProvider>();
            DisplayNameMetadataProviderProxy provider = new DisplayNameMetadataProviderProxy(details);

            String actual = provider.BaseCreateModelMetadata(metaDetails).DisplayName;

            Assert.Null(actual);
        }

        #endregion
    }
}
