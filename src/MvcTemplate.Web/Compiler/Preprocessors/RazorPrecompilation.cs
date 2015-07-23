using Microsoft.AspNet.Mvc;
using System;

namespace MvcTemplate.Web.Compiler.Preprocessors
{
    public class RazorPreCompilation : RazorPreCompileModule
    {
        public RazorPreCompilation(IServiceProvider provider)
            : base(provider)
        {
        }
    }
}
