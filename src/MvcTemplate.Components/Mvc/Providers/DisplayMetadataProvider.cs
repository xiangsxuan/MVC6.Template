using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using MvcTemplate.Resources;

namespace MvcTemplate.Components.Mvc
{
    public class DisplayMetadataProvider : IDisplayMetadataProvider
    {
        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            if (context.Key.ContainerType != null && context.Key.MetadataKind == ModelMetadataKind.Property)
                context.DisplayMetadata.DisplayName = () => ResourceProvider.GetPropertyTitle(context.Key.ContainerType, context.Key.Name);
        }
    }
}
