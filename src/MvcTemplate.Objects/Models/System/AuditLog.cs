using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class AuditLog : BaseModel
    {
        [StringLength(36)]
        public String AccountId { get; set; }

        [Required]
        [StringLength(16)]
        public String Action { get; set; }

        [Required]
        [StringLength(64)]
        public String EntityName { get; set; }

        [Required]
        [StringLength(36)]
        public String EntityId { get; set; }

        [Required]
        public String Changes { get; set; }
    }
}
