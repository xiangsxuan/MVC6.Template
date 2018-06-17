using Microsoft.AspNetCore.Mvc.ModelBinding;
using MvcTemplate.Components.Alerts;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using MvcTemplate.Resources.Form;
using System;
using System.Linq.Expressions;

namespace MvcTemplate.Validators
{
    public abstract class BaseValidator : IValidator
    {
        public ModelStateDictionary ModelState { get; set; }
        public Int32 CurrentAccountId { get; set; }
        public AlertsContainer Alerts { get; set; }
        protected IUnitOfWork UnitOfWork { get; }

        protected BaseValidator(IUnitOfWork unitOfWork)
        {
            ModelState = new ModelStateDictionary();
            Alerts = new AlertsContainer();
            UnitOfWork = unitOfWork;
        }

        protected Boolean IsSpecified<TView>(TView view, Expression<Func<TView, Object>> property) where TView : BaseView
        {
            Boolean isSpecified = property.Compile().Invoke(view) != null;

            if (!isSpecified)
            {
                if (property.Body is UnaryExpression unary)
                    ModelState.AddModelError(property, String.Format(Validations.Required, ResourceProvider.GetPropertyTitle(unary.Operand)));
                else
                    ModelState.AddModelError(property, String.Format(Validations.Required, ResourceProvider.GetPropertyTitle(property)));
            }

            return isSpecified;
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
