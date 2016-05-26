using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace MvcTemplate.Components.Mvc
{
    public class TrimmingModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(String))
                return new TrimmingModelBinder();

            return null;
        }
    }
}
