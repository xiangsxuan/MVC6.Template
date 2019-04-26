using Renting.Components.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Renting.Objects
{
    public class ProfileDeleteView : BaseView
    {
        [Required]
        [NotTrimmed]
        [StringLength(32)]
        public String Password { get; set; }
    }
}
