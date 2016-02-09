using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class Account : BaseModel
    {
        [Required]
        [StringLength(32)]
        public String Username { get; set; }

        [Required]
        [StringLength(64)]
        public String Passhash { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public String Email { get; set; }

        public Boolean IsLocked { get; set; }

        [StringLength(36)]
        public String RecoveryToken { get; set; }
        public DateTime? RecoveryTokenExpirationDate { get; set; }

        [StringLength(36)]
        public String RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
