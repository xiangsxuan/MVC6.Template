using Microsoft.AspNetCore.Mvc;
using Renting.Components.Lookups;
using Renting.Components.Mvc;
using Renting.Components.Security;
using Renting.Data.Core;
using Renting.Objects;
using NonFactors.Mvc.Lookup;
using System;

namespace Renting.Controllers
{
    [AllowUnauthorized]
    public class LookupController : BaseController
    {
        private IUnitOfWork UnitOfWork { get; }

        public LookupController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        [NonAction]
        public virtual JsonResult GetData(MvcLookup lookup, LookupFilter filter)
        {
            lookup.Filter = filter;

            return Json(lookup.GetData());
        }

        [AjaxOnly]
        public JsonResult Role(LookupFilter filter)
        {
            return GetData(new MvcLookup<Role, RoleView>(UnitOfWork), filter);
        }

        protected override void Dispose(Boolean disposing)
        {
            UnitOfWork.Dispose();

            base.Dispose(disposing);
        }
    }
}
