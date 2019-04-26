using Renting.Objects;
using System;
using System.ComponentModel.DataAnnotations;

namespace Renting.Tests
{
    public class TestModel : BaseModel
    {
        [StringLength(128)]
        public String Title { get; set; }
    }
}
