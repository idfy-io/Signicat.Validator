using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Signicat.Validator.IdfyPades.Infrastructure.Swagger;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;

namespace Signicat.Validator.IdfyPades.Infrastructure
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

    }
}