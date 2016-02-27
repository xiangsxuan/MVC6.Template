using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class Log : BaseModel
    {
        public Int32? AccountId { get; set; }

        [Required]
        public String Message { get; set; }
    }
}
