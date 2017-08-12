using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Threading.Tasks;

namespace MvcTemplate.Components.Mvc
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class TruncatedAttribute : ModelBinderAttribute, IModelBinder
    {
        public TruncatedAttribute()
        {
            BinderType = GetType();
        }

        public async Task BindModelAsync(ModelBindingContext context)
        {
            await new SimpleTypeModelBinder(typeof(DateTime?)).BindModelAsync(context);

            if (context.Result.IsModelSet)
                context.Result = ModelBindingResult.Success((context.Result.Model as DateTime?)?.Date);
        }
    }
}
