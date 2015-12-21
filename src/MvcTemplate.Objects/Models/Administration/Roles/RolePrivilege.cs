using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class RolePermission : BaseModel
    {
        [Required]
        [StringLength(128)]
        public String RoleId { get; set; }
        public virtual Role Role { get; set; }

        [Required]
        [StringLength(128)]
        public String PermissionId { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
