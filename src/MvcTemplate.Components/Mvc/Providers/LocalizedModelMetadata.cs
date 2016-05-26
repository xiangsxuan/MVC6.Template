using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using MvcTemplate.Resources;
using System;

namespace MvcTemplate.Components.Mvc
{
    public class LocalizedModelMetadata : DefaultModelMetadata
    {
        public String LocalizedDisplayName { get; }
        public override String DisplayName => LocalizedDisplayName;

        public LocalizedModelMetadata(IModelMetadataProvider provider, ICompositeMetadataDetailsProvider detailsProvider, DefaultMetadataDetails details)
            : base(provider, detailsProvider, details)
        {
            if (ContainerType == null)
                LocalizedDisplayName = base.DisplayName;
            else
                LocalizedDisplayName = ResourceProvider.GetPropertyTitle(ContainerType, PropertyName);
        }
    }
}