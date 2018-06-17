using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace MvcTemplate.Components.Mvc
{
    public class TrimmingModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return context.Metadata.ModelType == typeof(String) ? new TrimmingModelBinder() : null;
        }
    }
}
