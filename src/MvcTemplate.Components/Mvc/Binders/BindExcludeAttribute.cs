using Microsoft.AspNet.Mvc.ModelBinding;
using System;

namespace MvcTemplate.Components.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class BindExcludeIdAttribute : Attribute, IPropertyBindingPredicateProvider
    {
        public Func<ModelBindingContext, String, Boolean> PropertyFilter { get; private set; }

        public BindExcludeIdAttribute()
        {
            PropertyFilter = (context, property) => property != "Id";
        }
    }
}
