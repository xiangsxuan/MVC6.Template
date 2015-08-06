using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class AccountEditView : BaseView
    {
        [Required]
        public String Username { get; set; }

        [Required]
        public String Email { get; set; }

        public Boolean IsLocked { get; set; }

        public String RoleId { get; set; }
    }
}
