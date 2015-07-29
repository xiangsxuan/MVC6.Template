using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class RolePrivilege : BaseModel
    {
        [Required]
        [StringLength(128)]
        public String RoleId { get; set; }
        public virtual Role Role { get; set; }

        [Required]
        [StringLength(128)]
        public String PrivilegeId { get; set; }
        public virtual Privilege Privilege { get; set; }
    }
}
