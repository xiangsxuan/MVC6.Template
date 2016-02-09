using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class Log : BaseModel
    {
        [StringLength(64)]
        public String AccountId { get; set; }

        [Required]
        public String Message { get; set; }
    }
}
