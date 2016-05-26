using Microsoft.AspNetCore.Mvc.Filters;
using MvcTemplate.Services;
using System;

namespace MvcTemplate.Controllers
{
    public abstract class ServicedController<TService> : BaseController
        where TService : IService
    {
        public TService Service { get; }
        private Boolean Disposed { get; set; }

        protected ServicedController(TService service)
        {
            Service = service;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            Service.CurrentAccountId = CurrentAccountId;
        }

        protected override void Dispose(Boolean disposing)
        {
            if (Disposed) return;

            Service.Dispose();
            Disposed = true;

            base.Dispose(disposing);
        }
    }
}
