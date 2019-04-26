﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Renting.Components.Mvc
{
    public class ErrorPagesMiddleware
    {
        private ILogger Logger { get; }
        private ILanguages Languages { get; }
        private RequestDelegate Next { get; }

        public ErrorPagesMiddleware(RequestDelegate next, ILanguages languages, ILogger<ErrorPagesMiddleware> logger)
        {
            Next = next;
            Logger = logger;
            Languages = languages;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await Next(context);

                if (!context.Response.HasStarted && context.Response.StatusCode == StatusCodes.Status404NotFound)
                    View(context, "/home/not-found");
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "An unhandled exception has occurred while executing the request.");

                View(context, "/home/error");
            }
        }

        private async void View(HttpContext context, String path)
        {
            String originalPath = context.Request.Path;
            Match abbreviation = Regex.Match(originalPath, "^/(\\w{2})(/|$)");

            try
            {
                if (abbreviation.Success)
                {
                    Language language = Languages[abbreviation.Groups[1].Value];
                    if (language != Languages.Default)
                        context.Request.Path = $"/{language.Abbreviation}{path}";
                }
                else
                {
                    context.Request.Path = path;
                }

                await Next(context);
            }
            finally
            {
                context.Request.Path = originalPath;
            }
        }
    }
}
