using MvcTemplate.Components.Extensions;
using NonFactors.Mvc.Lookup;
using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class RoleView : BaseView
    {
        [Required]
        [LookupColumn]
        [StringLength(128)]
        public String Title { get; set; }

        public JsTree Permissions { get; set; }

        public RoleView()
        {
            Permissions = new JsTree();
        }
    }
}
