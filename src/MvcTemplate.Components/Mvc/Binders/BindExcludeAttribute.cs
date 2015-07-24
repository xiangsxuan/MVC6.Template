using Microsoft.AspNet.Mvc.ModelBinding;
using System;

namespace MvcTemplate.Components.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter)]
    public class BindExcludeIdAttribute : Attribute, IPropertyBindingPredicateProvider
    {
        public Func<ModelBindingContext, String, Boolean> PropertyFilter { get; }

        public BindExcludeIdAttribute()
        {
            PropertyFilter = (context, property) => property != "Id";
        }
    }
}
