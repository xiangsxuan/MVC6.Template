using Microsoft.AspNetCore.Mvc;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using NonFactors.Mvc.Lookup;
using System;
using System.Linq;
using System.Reflection;

namespace MvcTemplate.Components.Lookups
{
    public class MvcLookup<TModel, TView> : MvcLookup<TView>
        where TModel : BaseModel
        where TView : BaseView
    {
        protected IUnitOfWork UnitOfWork { get; }

        public MvcLookup(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        public MvcLookup(IUrlHelper url)
        {
            String view = typeof(TView).Name.Replace("View", "");
            Url = url.Action(view, Prefix, new { area = "" });
            Title = ResourceProvider.GetLookupTitle(view);
        }

        public override String GetColumnHeader(PropertyInfo property)
        {
            return ResourceProvider.GetPropertyTitle(typeof(TView), property.Name) ?? "";
        }
        public override String GetColumnCssClass(PropertyInfo property)
        {
            Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (type.IsEnum)
                return "text-left";

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
                case TypeCode.Boolean:
                case TypeCode.DateTime:
                    return "text-center";
                default:
                    return "text-left";
            }
        }

        public override IQueryable<TView> GetModels()
        {
            return UnitOfWork.Select<TModel>().To<TView>();
        }
    }
}
