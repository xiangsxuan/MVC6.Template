using Humanizer;
using MvcTemplate.Objects;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MvcTemplate.Web.Templates
{
    public class ModuleModel
    {
        public String Model { get; }
        public String Models { get; }
        public String ModelVarName { get; }
        public String ModelShortName { get; }

        public String View { get; }

        public String Service { get; }
        public String IService { get; }
        public String ServiceTests { get; }

        public String Validator { get; }
        public String IValidator { get; }
        public String ValidatorTests { get; }

        public String ControllerTestsNamespace { get; }
        public String ControllerNamespace { get; }
        public String ControllerTests { get; }
        public String Controller { get; }

        public String Area { get; }

        public PropertyInfo[] Properties { get; set; }
        public PropertyInfo[] AllProperties { get; set; }

        public ModuleModel(String model, String controller, String area)
        {
            ModelShortName = Regex.Split(model, "(?=[A-Z])").Last();
            ModelVarName = ModelShortName.ToLower();
            Models = model.Pluralize(false);
            Model = model;

            View = $"{Model}View";

            ServiceTests = $"{Model}ServiceTests";
            IService = $"I{Model}Service";
            Service = $"{Model}Service";

            ValidatorTests = $"{Model}ValidatorTests";
            IValidator = $"I{Model}Validator";
            Validator = $"{Model}Validator";

            ControllerTestsNamespace = "MvcTemplate.Tests.Unit.Controllers" + (!String.IsNullOrWhiteSpace(area) ? $".{area}" : "");
            ControllerNamespace = "MvcTemplate.Controllers" + (!String.IsNullOrWhiteSpace(area) ? $".{area}" : "");
            ControllerTests = $"{controller}ControllerTests";
            Controller = $"{controller}Controller";

            Area = area;

            AllProperties = typeof(BaseModel)
                .Assembly
                .GetTypes()
                .SingleOrDefault(type => type.Name.Equals(Model, StringComparison.OrdinalIgnoreCase))?
                .GetProperties(BindingFlags.Public | BindingFlags.Instance) ?? typeof(BaseModel).GetProperties();
            Properties = typeof(BaseModel)
                .Assembly
                .GetTypes()
                .SingleOrDefault(type => type.Name.Equals(Model, StringComparison.OrdinalIgnoreCase))?
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly) ?? new PropertyInfo[0];
        }
    }
}
