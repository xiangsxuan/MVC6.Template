using Renting.Objects;
using System;
using System.ComponentModel.DataAnnotations;

namespace Renting.Tests
{
    public class TestView : BaseView
    {
        [StringLength(128)]
        public String Title { get; set; }
    }
}
