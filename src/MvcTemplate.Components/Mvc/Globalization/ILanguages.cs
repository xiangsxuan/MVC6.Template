using System;

namespace MvcTemplate.Components.Mvc
{
    public interface ILanguages
    {
        Language Default { get; }
        Language Current { get; set; }
        Language[] Supported { get; }

        Language this[String abbreviation] { get; }
    }
}
