using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using MvcTemplate.Components.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class DisplayNameMetadataProviderProxy : DisplayNameMetadataProvider
    {
        public DisplayNameMetadataProviderProxy(ICompositeMetadataDetailsProvider detailsProvider)
            : base(detailsProvider)
        {
        }

        public ModelMetadata BaseCreateModelMetadata(DefaultMetadataDetails entry)
        {
            return CreateModelMetadata(entry);
        }
    }
}
