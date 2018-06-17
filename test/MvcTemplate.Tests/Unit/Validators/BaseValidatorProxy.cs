using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Validators;
using System;
using System.Linq.Expressions;

namespace MvcTemplate.Tests.Unit.Validators
{
    public class BaseValidatorProxy : BaseValidator
    {
        public BaseValidatorProxy(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public Boolean BaseIsSpecified<TView>(TView view, Expression<Func<TView, Object>> property) where TView : BaseView
        {
            return IsSpecified(view, property);
        }
    }
}
