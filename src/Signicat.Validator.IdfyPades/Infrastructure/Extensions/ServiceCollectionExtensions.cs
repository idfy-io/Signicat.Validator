using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Signicat.Validator.IdfyPades.Infrastructure.Swagger;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;

namespace Signicat.Validator.IdfyPades.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Swagger to specified IServiceCollection.
        /// </summary>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            var basePath = AppContext.BaseDirectory;
            var assemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            var xmlPath = Path.Combine(basePath, $"{assemblyName}.xml");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info()
                {
                    Title = "Signicat Validator",
                    Version = "v1",
                    Description =
                        "This is a Validator for Signed PDF's",
                    Contact = new Contact()
                    {
                        Name = "Signicat AS",
                        Url = "https://signicat.com",
                        Email = "support@signicat.com"
                    }
                });

                // Remove "DTO" from model names
                c.CustomSchemaIds(currentClass =>
                {
                    var name = currentClass.Name;
                    if (name.EndsWith("DTO", StringComparison.InvariantCultureIgnoreCase))
                    {
                        name = name.Substring(0, name.Length - "DTO".Length);
                    }

                    return name;
                });

                c.IncludeXmlComments(xmlPath);

                c.DescribeAllParametersInCamelCase();

                c.OperationFilter<ExamplesOperationFilter>();
                c.OperationFilter<SummaryOperationNameFilter>();

                c.TagActionsBy(api => SwaggerHelpers.CustomOrDefaultOperationGroup(api));

                c.UseReferencedDefinitionsForEnums();
            });

            return services;
        }





        /// <summary>
        /// Adds Serilog and the Seq sink to the specified IServiceCollection.
        /// </summary>
        public static IServiceCollection AddSeqLogger(this IServiceCollection services, SeqSettings settings,
            string serviceType)
        {
            var levelSwitch = new LoggingLevelSwitch();

            var logConfig = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ServiceType", serviceType)
                .Enrich.WithProperty("MachineName", Environment.MachineName)
                .WriteTo.Seq(settings.Url, apiKey: settings.ApiKey, controlLevelSwitch: levelSwitch);

            if (Environment.UserInteractive)
            {
                logConfig.WriteTo.ColoredConsole();
            }

            Log.Logger = logConfig.CreateLogger();

            services.AddSingleton(Log.Logger);

            Log.Logger.Information("PDF validator started");

            return services;
        }

        /// <summary>
        /// Adds CORS policies to the specified IServiceCollection.
        /// </summary>
        /// <param name="services"></param>        
        /// <returns></returns>
        public static IServiceCollection AddCorsPolicies(this IServiceCollection services)
        {
            services.AddCors(opts =>
            {
            

                opts.AddPolicy("External", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            return services;
        }

    }
}