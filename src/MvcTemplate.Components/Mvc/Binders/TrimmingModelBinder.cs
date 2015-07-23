using Microsoft.AspNet.Mvc.ModelBinding;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MvcTemplate.Components.Mvc
{
    public class TrimmingModelBinder : IModelBinder
    {
        public async Task<ModelBindingResult> BindModelAsync(ModelBindingContext context)
        {
            ValueProviderResult value = await context.ValueProvider.GetValueAsync(context.ModelName);
            if (value == null || value.AttemptedValue == null)
                return new ModelBindingResult(null, context.ModelName, false);

            Type containerType = context.ModelMetadata.ContainerType;
            if (containerType != null)
            {
                PropertyInfo property = containerType.GetProperty(context.ModelName);
                if (property.GetCustomAttribute<NotTrimmedAttribute>() != null)
                    return new ModelBindingResult(value.AttemptedValue, context.ModelName, true);
            }

            return new ModelBindingResult(value.AttemptedValue.Trim(), context.ModelName, true);
        }
    }
}
