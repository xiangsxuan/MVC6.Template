using Microsoft.AspNetCore.Http;
using MvcTemplate.Resources.Form;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcTemplate.Components.Mvc
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class AcceptFilesAttribute : ValidationAttribute
    {
        public String Extensions { get; private set; }

        public AcceptFilesAttribute(String extensions)
            : base(() => Validations.AcceptFiles)
        {
            Extensions = extensions;
        }

        public override String FormatErrorMessage(String name)
        {
            return String.Format(ErrorMessageString, name, Extensions);
        }
        public override Boolean IsValid(Object value)
        {
            IEnumerable<IFormFile> files = ToFiles(value);

            if (value == null)
                return true;

            if (files == null)
                return false;

            if (files.Any(file => file.FileName == null))
                return false;

            return files.All(file => Extensions.Split(',').Any(extension => file.FileName.EndsWith(extension)));
        }

        private IEnumerable<IFormFile> ToFiles(Object value)
        {
            IFormFile file = value as IFormFile;
            if (file != null)
                return new[] { file };

            return value as IEnumerable<IFormFile>;
        }
    }
}
