using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MvcTemplate.Components.Mvc
{
    public class TrimmingModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext context)
        {
            context.Result = BindModel(context);

            return Task.CompletedTask;
        }

        private ModelBindingResult BindModel(ModelBindingContext context)
        {
            ValueProviderResult result = context.ValueProvider.GetValue(context.ModelName);
            if (result == ValueProviderResult.None)
                return context.Result;

            String value = result.FirstValue;
            context.ModelState.SetModelValue(context.ModelName, result);
            PropertyInfo property = context.ModelMetadata.ContainerType?.GetProperty(context.ModelMetadata.PropertyName);

            if (property?.IsDefined(typeof(NotTrimmedAttribute), false) != true)
                value = value.Trim();

            if (value.Length == 0 && context.ModelMetadata.ConvertEmptyStringToNull)
                return ModelBindingResult.Success(null);

            return ModelBindingResult.Success(value);
        }
    }
}
