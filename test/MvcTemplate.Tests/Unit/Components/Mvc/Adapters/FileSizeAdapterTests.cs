using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
using MvcTemplate.Tests.Objects;
using System;
using System.Collections.Generic;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class FileSizeAdapterTests
    {
        private FileSizeAdapter adapter;
        private ClientModelValidationContext context;
        private Dictionary<String, String> attributes;

        public FileSizeAdapterTests()
        {
            attributes = new Dictionary<String, String>();
            adapter = new FileSizeAdapter(new FileSizeAttribute(12.25));
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "FileSize");
            context = new ClientModelValidationContext(new ActionContext(), metadata, provider, attributes);
        }

        #region AddValidation(ClientModelValidationContext context)

        [Fact]
        public void AddValidation_FileSize()
        {
            adapter.AddValidation(context);

            Assert.Equal(3, attributes.Count);
            Assert.Equal("true", attributes["data-val"]);
            Assert.Equal("12845056.00", attributes["data-val-filesize-max"]);
            Assert.Equal(String.Format(Validations.FileSize, "FileSize", 12.25), attributes["data-val-filesize"]);
        }

        #endregion

        #region GetErrorMessage(ModelValidationContextBase context)

        [Fact]
        public void GetErrorMessage_FileSize()
        {
            String expected = String.Format(Validations.FileSize, "FileSize", 12.25);
            String actual = adapter.GetErrorMessage(context);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
