using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace MvcTemplate.Components.Mvc
{
    public class DisplayNameMetadataProvider : DefaultModelMetadataProvider
    {
        public DisplayNameMetadataProvider(ICompositeMetadataDetailsProvider detailsProvider)
            : base(detailsProvider)
        {
        }

        protected override ModelMetadata CreateModelMetadata(DefaultMetadataDetails entry)
        {
            return new LocalizedModelMetadata(this, DetailsProvider, entry);
        }
    }
}
