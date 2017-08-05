using Microsoft.AspNetCore.Mvc.ModelBinding;
using MvcTemplate.Components.Mvc;
using System;
using System.Collections.Generic;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class ModelStateDictionaryExtensionsTests
    {
        private ModelStateDictionary modelState;

        public ModelStateDictionaryExtensionsTests()
        {
            modelState = new ModelStateDictionary();
        }

        #region Errors(this ModelStateDictionary modelState)

        [Fact]
        public void Errors_FromModelState()
        {
            modelState.AddModelError("Empty", "");
            modelState.AddModelError("Error", "Error");
            modelState.AddModelError("EmptyErrors", "");
            modelState.AddModelError("EmptyErrors", "E");
            modelState.SetModelValue("NoErrors", "A", "A");
            modelState.AddModelError("TwoErrors", "Error1");
            modelState.AddModelError("TwoErrors", "Error2");
            modelState.AddModelError("WhitespaceErrors", "       ");
            modelState.AddModelError("WhitespaceErrors", "Whitespace");

            Dictionary<String, String> actual = modelState.Errors();
            Dictionary<String, String> expected = new Dictionary<String, String>
            {
                ["Empty"] = null,
                ["Error"] = "Error",
                ["EmptyErrors"] = "E",
                ["TwoErrors"] = "Error1",
                ["WhitespaceErrors"] = "       "
            };

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
