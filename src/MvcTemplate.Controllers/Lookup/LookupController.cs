using Microsoft.AspNet.Mvc;
using MvcTemplate.Components.Lookups;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using NonFactors.Mvc.Lookup;
using System;

namespace MvcTemplate.Controllers
{
    [AllowUnauthorized]
    public class LookupController : BaseController
    {
        private IUnitOfWork UnitOfWork { get; set; }
        private Boolean Disposed { get; set; }

        public LookupController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        [NonAction]
        public virtual JsonResult GetData(AbstractLookup lookup, LookupFilter filter)
        {
            lookup.CurrentFilter = filter;

            return Json(lookup.GetData());
        }

        [AjaxOnly]
        public JsonResult Role(LookupFilter filter)
        {
            return GetData(new BaseLookup<Role, RoleView>(UnitOfWork), filter);
        }

        protected override void Dispose(Boolean disposing)
        {
            if (Disposed) return;

            UnitOfWork.Dispose();
            Disposed = true;

            base.Dispose(disposing);
        }
    }
}
