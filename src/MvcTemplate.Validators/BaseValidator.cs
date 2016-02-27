using Microsoft.AspNet.Mvc.ModelBinding;
using MvcTemplate.Components.Alerts;
using MvcTemplate.Data.Core;
using System;

namespace MvcTemplate.Validators
{
    public abstract class BaseValidator : IValidator
    {
        public ModelStateDictionary ModelState { get; set; }
        public Int32 CurrentAccountId { get; set; }
        public AlertsContainer Alerts { get; set; }
        protected IUnitOfWork UnitOfWork { get; }
        private Boolean Disposed { get; set; }

        protected BaseValidator(IUnitOfWork unitOfWork)
        {
            ModelState = new ModelStateDictionary();
            Alerts = new AlertsContainer();
            UnitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            if (Disposed) return;

            UnitOfWork.Dispose();

            Disposed = true;
        }
    }
}
