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
    public class AcceptFilesAdapterTests
    {
        private AcceptFilesAdapter adapter;
        private ClientModelValidationContext context;
        private Dictionary<String, String> attributes;

        public AcceptFilesAdapterTests()
        {
            attributes = new Dictionary<String, String>();
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            adapter = new AcceptFilesAdapter(new AcceptFilesAttribute(".docx,.rtf"));
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "AcceptFiles");
            context = new ClientModelValidationContext(new ActionContext(), metadata, provider, attributes);
        }

        #region AddValidation(ClientModelValidationContext context)

        [Fact]
        public void AddValidation_AcceptFiles()
        {
            adapter.AddValidation(context);

            Assert.Equal(3, attributes.Count);
            Assert.Equal("true", attributes["data-val"]);
            Assert.Equal(".docx,.rtf", attributes["data-val-acceptfiles-extensions"]);
            Assert.Equal(String.Format(Validations.AcceptFiles, "AcceptFiles", ".docx,.rtf"), attributes["data-val-acceptfiles"]);
        }

        #endregion

        #region GetErrorMessage(ModelValidationContextBase context)

        [Fact]
        public void GetErrorMessage_AcceptFiles()
        {
            String expected = String.Format(Validations.AcceptFiles, "AcceptFiles", ".docx,.rtf");
            String actual = adapter.GetErrorMessage(context);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
