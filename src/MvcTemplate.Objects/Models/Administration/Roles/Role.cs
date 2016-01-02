using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class Role : BaseModel
    {
        [Required]
        [StringLength(128)]
        public String Title { get; set; }

        public virtual IList<RolePermission> Permissions { get; set; }
    }
}
