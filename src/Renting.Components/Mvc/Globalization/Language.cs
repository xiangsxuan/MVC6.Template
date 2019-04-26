using System;
using System.Globalization;

namespace Renting.Components.Mvc
{
    public class Language
    {
        public String Name { get; set; }
        public String Abbreviation { get; set; }
        public CultureInfo Culture { get; set; }
    }
}
