using Microsoft.AspNetCore.Http;
using MvcTemplate.Resources.Form;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MvcTemplate.Components.Mvc
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class FileSizeAttribute : ValidationAttribute
    {
        public Decimal MaximumMB { get; private set; }

        public FileSizeAttribute(Double maximumMB)
            : base(() => Validations.FileSize)
        {
            MaximumMB = Convert.ToDecimal(maximumMB);
        }

        public override String FormatErrorMessage(String name)
        {
            return String.Format(ErrorMessageString, name, MaximumMB);
        }
        public override Boolean IsValid(Object value)
        {
            IEnumerable<IFormFile> files = ToFiles(value);

            return files == null || files.Sum(file => file == null ? 0 : file.Length) <= MaximumMB * 1024 * 1024;
        }

        private IEnumerable<IFormFile> ToFiles(Object value)
        {
            IFormFile file = value as IFormFile;
            if (file != null) return new[] { file };

            return value as IEnumerable<IFormFile>;
        }
    }
}
