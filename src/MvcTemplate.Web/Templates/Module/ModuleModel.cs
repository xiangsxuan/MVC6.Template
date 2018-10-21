using Humanizer;
using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
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
        public Dictionary<PropertyInfo, String> Relations { get; set; }

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

            Type type = typeof(BaseModel).Assembly.GetType("MvcTemplate.Objects." + model) ?? typeof(BaseModel);
            PropertyInfo[] modelProperties = type.GetProperties();

            AllProperties = modelProperties.Where(property => property.PropertyType.Namespace == "System").ToArray();
            Properties = AllProperties.Where(property => property.DeclaringType.Name == model).ToArray();
            Relations = Properties
                .ToDictionary(
                    property => property,
                    property => modelProperties.SingleOrDefault(relation =>
                        property.Name.EndsWith("Id") &&
                        relation.PropertyType.Assembly == type.Assembly &&
                        relation.Name == property.Name.Remove(property.Name.Length - 2))?.PropertyType.Name);
        }
    }
}
