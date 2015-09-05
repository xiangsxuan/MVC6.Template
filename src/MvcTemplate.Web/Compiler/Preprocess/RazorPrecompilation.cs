using Microsoft.AspNet.Mvc;
using System;

namespace MvcTemplate.Web.Compiler.Preprocess
{
    public class RazorPreCompilation : RazorPreCompileModule
    {
        public RazorPreCompilation(IServiceProvider provider)
            : base(provider)
        {
        }
    }
}
