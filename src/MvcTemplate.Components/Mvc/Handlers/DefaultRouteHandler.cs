using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace MvcTemplate.Components.Mvc
{
    public class DefaultRouteHandler : IRouter
    {
        private IRouter Router { get; }

        public DefaultRouteHandler(IRouter router)
        {
            Router = router;
        }

        public async Task RouteAsync(RouteContext context)
        {
            await Router.RouteAsync(context);

            if (context.Handler == null)
                context.Handler = (http) =>
                {
                    http.Response.StatusCode = 404;

                    return Task.CompletedTask;
                };
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return Router.GetVirtualPath(context);
        }
    }
}
