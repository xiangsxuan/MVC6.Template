using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MvcTemplate.Components.Mvc
{
    public class TrimmingModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            bindingContext.Result = BindModel(bindingContext);

            return Task.CompletedTask;
        }

        private ModelBindingResult? BindModel(ModelBindingContext context)
        {
            ValueProviderResult result = context.ValueProvider.GetValue(context.ModelName);
            if (result == ValueProviderResult.None)
                return null;

            String value = result.FirstValue;
            context.ModelState.SetModelValue(context.ModelName, result);
            PropertyInfo property = context.ModelMetadata.ContainerType?.GetProperty(context.ModelMetadata.PropertyName);

            if (property?.IsDefined(typeof(NotTrimmedAttribute), false) != true)
                value = value.Trim();

            if (context.ModelMetadata.ConvertEmptyStringToNull && value.Length == 0)
                return ModelBindingResult.Success(context.ModelName, null);

            return ModelBindingResult.Success(context.ModelName, value);
        }
    }
}
