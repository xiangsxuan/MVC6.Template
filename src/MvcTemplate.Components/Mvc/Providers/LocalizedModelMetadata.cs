using Microsoft.AspNet.Mvc.ModelBinding.Metadata;
using MvcTemplate.Resources;
using System;

namespace MvcTemplate.Components.Mvc
{
    public class LocalizedModelMetadata : DefaultModelMetadata
    {
        public String LocalizedDisplayName
        {
            get;
            private set;
        }
        public override String DisplayName
        {
            get
            {
                return LocalizedDisplayName;
            }
        }

        public LocalizedModelMetadata(DisplayNameMetadataProvider provider, ICompositeMetadataDetailsProvider detailsProvider, DefaultMetadataDetails details)
            : base(provider, detailsProvider, details)
        {
            if (ContainerType == null)
                LocalizedDisplayName = base.DisplayName;
            else
                LocalizedDisplayName = ResourceProvider.GetPropertyTitle(ContainerType, PropertyName);
        }
    }
}