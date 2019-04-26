using System;
using System.ComponentModel.DataAnnotations;

namespace Renting.Components.Mvc.Tests
{
    public class TagHelperModel
    {
        [Required]
        public String Required { get; set; }
        public String NotRequired { get; set; }
        public Int64 RequiredValue { get; set; }
        public Int64? NotRequiredNullableValue { get; set; }
    }
}
