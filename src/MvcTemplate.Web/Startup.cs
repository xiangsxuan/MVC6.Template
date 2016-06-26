using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcTemplate.Components.Logging;
using MvcTemplate.Components.Mail;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Controllers;
using MvcTemplate.Data.Core;
using MvcTemplate.Data.Migrations;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using NonFactors.Mvc.Grid;
using System;
using System.Collections.Generic;

namespace MvcTemplate.Web
{
    public class Startup
    {
        private IConfiguration Config { get; }

        public Startup(IHostingEnvironment env)
        {
            Config = new ConfigurationBuilder()
                .AddInMemoryCollection(new[] { new KeyValuePair<String, String>("Application:Path", env.ContentRootPath) })
                .AddJsonFile("configuration.json")
                .SetBasePath(env.ContentRootPath)
                .Build();
        }
        public void Configure(IApplicationBuilder app)
        {
            RegisterMiddleware(app);
            RegisterServices(app);
            RegisterRoute(app);

            SeedData(app);
        }
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterMvc(services);
            RegisterServices(services);
            RegisterLowercaseUrls(services);
        }

        public virtual void RegisterMvc(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddMvcOptions(options => options.Filters.Add(typeof(AuthorizationFilter)))
                .AddMvcOptions(options => options.Filters.Add(typeof(GlobalizationFilter)))
                .AddRazorOptions(options => options.ViewLocationExpanders.Add(new ViewLocationExpander()))
                .AddMvcOptions(options => options.ModelBinderProviders.Insert(0, new TrimmingModelBinderProvider()));
        }
        public virtual void RegisterServices(IServiceCollection services)
        {
            services.AddMvcGrid();
            services.AddSession();
            services.AddSingleton(Config);

            services.AddTransient<DbContext, Context>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IHasher, BCrypter>();
            services.AddTransient<IMailClient, SmtpMailClient>();
            services.AddTransient<ILogger>(provider => new Logger(
                    provider.GetService<IConfiguration>(),
                    provider.GetService<IHttpContextAccessor>().HttpContext?.User.Id()));

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IModelMetadataProvider, DisplayNameMetadataProvider>();
            services.AddSingleton<IValidationAttributeAdapterProvider, GlobalizedValidationAdapterProvider>();

            services.AddSingleton<IGlobalizationProvider, GlobalizationProvider>();
            services.AddSingleton<IAuthorizationProvider>(provider =>
                new AuthorizationProvider(typeof(BaseController).Assembly, provider));

            services.AddTransient<IMvcSiteMapParser, MvcSiteMapParser>();
            services.AddSingleton<IMvcSiteMapProvider, MvcSiteMapProvider>();

            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IAccountService, AccountService>();

            services.AddTransient<IRoleValidator, RoleValidator>();
            services.AddTransient<IAccountValidator, AccountValidator>();
        }
        public virtual void RegisterLowercaseUrls(IServiceCollection services)
        {
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        }

        public virtual void RegisterMiddleware(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionFilterMiddleware>();
        }
        public virtual void RegisterServices(IApplicationBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                Events = new GlobalizedAuthenticationEvents(),
                AuthenticationScheme = "Cookies",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseSession();
        }
        public virtual void RegisterRoute(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "DefaultMultilingualArea",
                    "{language}/{area:exists}/{controller}/{action}/{id?}",
                    new { controller = "Home", action = "Index" },
                    new { language = "lt" });

                routes.MapRoute(
                    "DefaultArea",
                    "{area:exists}/{controller}/{action}/{id?}",
                    new { language = "en", controller = "Home", action = "Index" },
                    new { language = "en" });

                routes.MapRoute(
                    "DefaultMultilingual",
                    "{language}/{controller}/{action}/{id?}",
                    new { controller = "Home", action = "Index" },
                    new { language = "lt" });

                routes.MapRoute(
                    "Default",
                    "{controller}/{action}/{id?}",
                    new { language = "en", controller = "Home", action = "Index" },
                    new { language = "en" });
            });
        }

        public virtual void SeedData(IApplicationBuilder app)
        {
            using (Configuration configuration = new Configuration(app.ApplicationServices.GetService<DbContext>()))
                configuration.Seed();
        }
    }
}
