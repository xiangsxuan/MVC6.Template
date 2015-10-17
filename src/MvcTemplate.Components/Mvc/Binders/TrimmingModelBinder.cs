using Microsoft.AspNet.Mvc.ModelBinding;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MvcTemplate.Components.Mvc
{
    public class TrimmingModelBinder : IModelBinder
    {
        public Task<ModelBindingResult> BindModelAsync(ModelBindingContext context)
        {
            if (context.ModelType == typeof(String))
            {
                ValueProviderResult result = context.ValueProvider.GetValue(context.ModelName);
                if (result == ValueProviderResult.None)
                    return ModelBindingResult.NoResultAsync;

                Type containerType = context.ModelMetadata.ContainerType;
                PropertyInfo property = containerType?.GetProperty(context.ModelName);
                context.ModelState.SetModelValue(context.ModelName, result.FirstValue, result.FirstValue);

                if (property?.IsDefined(typeof(NotTrimmedAttribute), false) == true)
                    return ModelBindingResult.SuccessAsync(context.ModelName, result.FirstValue);

                return ModelBindingResult.SuccessAsync(context.ModelName, result.FirstValue.Trim());
            }

            return ModelBindingResult.NoResultAsync;
        }
    }
}
