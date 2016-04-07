using Microsoft.AspNet.Mvc.ModelBinding;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace MvcTemplate.Components.Mvc
{
    public class TrimmingModelBinder : IModelBinder
    {
        public Task<ModelBindingResult> BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(String))
                return ModelBindingResult.NoResultAsync;

            ValueProviderResult result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (result == ValueProviderResult.None)
                return ModelBindingResult.NoResultAsync;

            Type containerType = bindingContext.ModelMetadata.ContainerType;
            PropertyInfo property = containerType?.GetProperty(bindingContext.ModelMetadata.PropertyName);

            if (property?.IsDefined(typeof(NotTrimmedAttribute), false) == true)
                return ModelBindingResult.NoResultAsync;

            String value = result.FirstValue.Trim();
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, result);
            if (bindingContext.ModelMetadata.ConvertEmptyStringToNull && value.Length == 0)
                return ModelBindingResult.SuccessAsync(bindingContext.ModelName, null);

            return ModelBindingResult.SuccessAsync(bindingContext.ModelName, value);
        }
    }
}
