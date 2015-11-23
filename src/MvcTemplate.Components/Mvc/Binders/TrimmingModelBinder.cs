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
            if (context.ModelType != typeof(String))
                return ModelBindingResult.NoResultAsync;

            ValueProviderResult result = context.ValueProvider.GetValue(context.ModelName);
            if (result == ValueProviderResult.None)
                return ModelBindingResult.NoResultAsync;

            Type containerType = context.ModelMetadata.ContainerType;
            PropertyInfo property = containerType?.GetProperty(context.ModelName);

            if (property?.IsDefined(typeof(NotTrimmedAttribute), false) == true)
                return ModelBindingResult.NoResultAsync;

            String value = result.FirstValue.Trim();
            context.ModelState.SetModelValue(context.ModelName, result);
            if (context.ModelMetadata.ConvertEmptyStringToNull && value.Length == 0)
                return ModelBindingResult.SuccessAsync(context.ModelName, null);

            return ModelBindingResult.SuccessAsync(context.ModelName, value);
        }
    }
}
