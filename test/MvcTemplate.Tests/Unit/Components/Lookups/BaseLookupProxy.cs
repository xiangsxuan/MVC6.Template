using Microsoft.AspNetCore.Mvc;
using MvcTemplate.Components.Lookups;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using System;
using System.Linq;
using System.Reflection;

namespace MvcTemplate.Tests.Unit.Components.Lookups
{
    public class BaseLookupProxy<TModel, TView> : BaseLookup<TModel, TView>
        where TModel : BaseModel
        where TView : BaseView
    {
        public IUnitOfWork BaseUnitOfWork => UnitOfWork;

        public BaseLookupProxy(IUrlHelper url) : base(url)
        {
        }
        public BaseLookupProxy(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public String BaseGetColumnHeader(PropertyInfo property)
        {
            return GetColumnHeader(property);
        }
        public String BaseGetColumnCssClass(PropertyInfo property)
        {
            return GetColumnCssClass(property);
        }

        public IQueryable<TView> BaseGetModels()
        {
            return GetModels();
        }

        public IQueryable<TView> BaseFilterById(IQueryable<TView> models)
        {
            return FilterById(models);
        }
    }
}
