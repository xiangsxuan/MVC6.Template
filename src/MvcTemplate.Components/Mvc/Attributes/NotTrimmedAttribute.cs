using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Threading.Tasks;

namespace MvcTemplate.Components.Mvc
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class NotTrimmedAttribute : ModelBinderAttribute, IModelBinder
    {
        public NotTrimmedAttribute()
        {
            BinderType = GetType();
        }

        public Task BindModelAsync(ModelBindingContext context)
        {
            return new SimpleTypeModelBinder(typeof(String)).BindModelAsync(context);
        }
    }
}
