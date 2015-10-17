using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using MvcTemplate.Components.Security;
using System;

namespace MvcTemplate.Components.Mvc
{
    [HtmlTargetElement("authorize", Attributes = "comp-area")]
    [HtmlTargetElement("authorize", Attributes = "comp-action")]
    [HtmlTargetElement("authorize", Attributes = "comp-controller")]
    public class AuthorizeTagHelper : TagHelper
    {
        [HtmlAttributeName("comp-area")]
        public String Area { get; set; }

        [HtmlAttributeName("comp-action")]
        public String Action { get; set; }

        [HtmlAttributeName("comp-controller")]
        public String Controller { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            if (Authorization.Provider == null) return;

            String accountId = ViewContext.HttpContext.User.Identity.Name;
            String area = Area ?? ViewContext.RouteData.Values["area"] as String;
            String action = Action ?? ViewContext.RouteData.Values["action"] as String;
            String controller = Controller ?? ViewContext.RouteData.Values["controller"] as String;

            if (!Authorization.Provider.IsAuthorizedFor(accountId, area, controller, action))
                output.SuppressOutput();
        }
    }
}
