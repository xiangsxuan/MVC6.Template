using Microsoft.AspNet.Mvc.ModelBinding;
using MvcTemplate.Components.Alerts;
using System;

namespace MvcTemplate.Validators
{
    public interface IValidator : IDisposable
    {
        ModelStateDictionary ModelState { get; set; }
        String CurrentAccountId { get; set; }
        AlertsContainer Alerts { get; set; }
    }
}
