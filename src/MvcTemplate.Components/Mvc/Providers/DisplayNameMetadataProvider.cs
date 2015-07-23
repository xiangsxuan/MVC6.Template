using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.ModelBinding.Metadata;

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
