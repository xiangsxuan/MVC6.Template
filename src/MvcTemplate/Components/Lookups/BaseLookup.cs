using Microsoft.AspNet.Mvc;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using NonFactors.Mvc.Lookup;
using System;
using System.Linq;
using System.Reflection;

namespace MvcTemplate.Components.Lookups
{
    public class BaseLookup<TModel, TView> : GenericLookup<TView>
        where TModel : BaseModel
        where TView : BaseView
    {
        protected IUnitOfWork UnitOfWork { get; set; }

        public BaseLookup(IUrlHelper url)
        {
            String view = typeof(TView).Name.Replace("View", "");

            DialogTitle = ResourceProvider.GetLookupTitle(view);
            LookupUrl = url.Action(view, Prefix, new { area = "" });
        }
        public BaseLookup(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        protected override String GetColumnHeader(PropertyInfo property)
        {
            LookupColumnAttribute column = property.GetCustomAttribute<LookupColumnAttribute>(false);
            if (column != null && column.Relation != null)
                return GetColumnHeader(property.PropertyType.GetProperty(column.Relation));

            return ResourceProvider.GetPropertyTitle(typeof(TView), property.Name) ?? "";
        }
        protected override String GetColumnCssClass(PropertyInfo property)
        {
            Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (type.IsEnum) return "text-left";

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return "text-right";
                case TypeCode.DateTime:
                    return "text-center";
                default:
                    return "text-left";
            }
        }

        protected override IQueryable<TView> GetModels()
        {
            return UnitOfWork.Select<TModel>().To<TView>();
        }

        protected override IQueryable<TView> FilterById(IQueryable<TView> models)
        {
            return UnitOfWork.Select<TModel>().Where(model => model.Id == CurrentFilter.Id).To<TView>();
        }
    }
}
