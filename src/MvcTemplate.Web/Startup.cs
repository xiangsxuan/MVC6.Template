using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcTemplate.Components.Extensions;
using MvcTemplate.Components.Logging;
using MvcTemplate.Components.Mail;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Controllers;
using MvcTemplate.Data.Core;
using MvcTemplate.Data.Logging;
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
            Dictionary<String, String> config = new Dictionary<String, String>();
            config.Add("Application:Path", env.ContentRootPath);
            config.Add("Application:Env", env.EnvironmentName);

            Config = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("configuration.json")
                .AddJsonFile($"configuration.{env.EnvironmentName.ToLower()}.json", optional: true)
                .AddInMemoryCollection(config)
                .Build();
        }
        public void Configure(IApplicationBuilder app)
        {
            RegisterServices(app);
            RegisterMvc(app);

            UpdateDatabase(app);
        }
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterMvc(services);
            RegisterServices(services);
            RegisterLowercaseUrls(services);
            RegisterSecureResponse(services);
        }

        public void RegisterMvc(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddMvcOptions(options => options.Filters.Add(typeof(LanguageFilter)))
                .AddMvcOptions(options => options.Filters.Add(typeof(AuthorizationFilter)))
                .AddMvcOptions(options => new ModelMessagesProvider(options.ModelBindingMessageProvider))
                .AddRazorOptions(options => options.ViewLocationExpanders.Add(new ViewLocationExpander()))
                .AddViewOptions(options => options.ClientModelValidatorProviders.Add(new DateValidatorProvider()))
                .AddMvcOptions(options => options.ModelMetadataDetailsProviders.Add(new DisplayMetadataProvider()))
                .AddViewOptions(options => options.ClientModelValidatorProviders.Add(new NumberValidatorProvider()))
                .AddMvcOptions(options => options.ModelBinderProviders.Insert(4, new TrimmingModelBinderProvider()));

            services.AddAuthentication("Cookies").AddCookie(authentication =>
            {
                authentication.Cookie.Name = Config["Cookies:Auth:Name"];
                authentication.Events = new AuthenticationEvents();
            });
        }
        public void RegisterServices(IServiceCollection services)
        {
            services.AddMvcGrid();
            services.AddSession();
            services.AddSingleton(Config);

            services.AddTransient<Configuration>();
            services.AddTransient<DbContext, Context>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<ILogger, Logger>();
            services.AddTransient<IAuditLogger>(provider =>
                new AuditLogger(provider.GetService<DbContext>(),
                provider.GetRequiredService<IHttpContextAccessor>().HttpContext?.User?.Id()));

            services.AddSingleton<IHasher, BCrypter>();
            services.AddSingleton<IMailClient, SmtpMailClient>();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IValidationAttributeAdapterProvider, ValidationAdapterProvider>();

            services.AddSingleton<ILanguages, Languages>();
            services.AddSingleton<IAuthorizationProvider>(provider =>
                new AuthorizationProvider(typeof(BaseController).Assembly, provider));

            services.AddSingleton<IMvcSiteMapParser, MvcSiteMapParser>();
            services.AddSingleton<IMvcSiteMapProvider, MvcSiteMapProvider>();

            services.AddTransientImplementations<IService>();
            services.AddTransientImplementations<IValidator>();
        }
        public void RegisterLowercaseUrls(IServiceCollection services)
        {
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        }
        public void RegisterSecureResponse(IServiceCollection services)
        {
            services.Configure<SessionOptions>(session => session.Cookie.Name = Config["Cookies:Session:Name"]);
            services.Configure<AntiforgeryOptions>(antiforgery =>
            {
                antiforgery.Cookie.Name = Config["Cookies:Antiforgery:Name"];
                antiforgery.FormFieldName = "_Token_";
            });
        }

        public void RegisterServices(IApplicationBuilder app)
        {
            if (Config["Application:Env"] == EnvironmentName.Development)
                app.UseMiddleware<DeveloperExceptionPageMiddleware>();
            else
                app.UseMiddleware<ErrorPagesMiddleware>();

            app.UseMiddleware<ExceptionFilterMiddleware>();
            app.UseMiddleware<SecureHeadersMiddleware>();

            app.UseAuthentication();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = (response) =>
                {
                    response.Context.Response.Headers["Cache-Control"] = "max-age=8640000";
                }
            });
            app.UseSession();
        }
        public void RegisterMvc(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "DefaultMultilingualArea",
                    "{language}/{area:exists}/{controller}/{action}/{id?}",
                    new { controller = "Home", action = "Index" },
                    new { id = "[0-9]*" });

                routes.MapRoute(
                    "DefaultArea",
                    "{area:exists}/{controller}/{action}/{id?}",
                    new { controller = "Home", action = "Index" },
                    new { id = "[0-9]*" });

                routes.MapRoute(
                    "DefaultMultilingual",
                    "{language}/{controller}/{action}/{id?}",
                    new { controller = "Home", action = "Index" },
                    new { id = "[0-9]*" });

                routes.MapRoute(
                    "Default",
                    "{controller}/{action}/{id?}",
                    new { controller = "Home", action = "Index" },
                    new { id = "[0-9]*" });
            });
        }

        public void UpdateDatabase(IApplicationBuilder app)
        {
            using (Configuration configuration = app.ApplicationServices.GetService<Configuration>())
                configuration.UpdateDatabase();
        }
    }
}
