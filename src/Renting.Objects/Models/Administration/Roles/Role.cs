﻿using Renting.Components.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Renting.Objects
{
    public class Role : BaseModel
    {
        [Required]
        [StringLength(128)]
        [Index(IsUnique = true)]
        public String Title { get; set; }

        public virtual List<Account> Accounts { get; set; }
        public virtual List<RolePermission> Permissions { get; set; }
    }
}
