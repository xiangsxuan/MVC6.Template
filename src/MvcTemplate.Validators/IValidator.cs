using Microsoft.AspNetCore.Mvc.ModelBinding;
using MvcTemplate.Components.Alerts;
using System;

namespace MvcTemplate.Validators
{
    public interface IValidator : IDisposable
    {
        ModelStateDictionary ModelState { get; set; }
        Int32 CurrentAccountId { get; set; }
        AlertsContainer Alerts { get; set; }
    }
}
