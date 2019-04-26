using Microsoft.AspNetCore.Mvc.ModelBinding;
using Renting.Components.Notifications;
using System;

namespace Renting.Validators
{
    public interface IValidator : IDisposable
    {
        ModelStateDictionary ModelState { get; set; }
        Int32 CurrentAccountId { get; set; }
        Alerts Alerts { get; set; }
    }
}
