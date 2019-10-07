using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Signicat.Validator.IdfyPades.Infrastructure;
using Signicat.Validator.IdfyPades.ValidatorService;

namespace Signicat.Validator.IdfyPades
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<AppSettings>(Configuration);
            AppSettings settings = Configuration.Get<AppSettings>();

            services.AddSeqLogger(settings.Seq, "web");
            services.AddMvcCore(mvc =>
            {
                mvc.InputFormatters.Clear();
                mvc.OutputFormatters.Clear();
                    

            });
            services.AddCorsPolicies();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddSwagger();
            

            services.AddSingleton<PDFValidator>(new PDFValidator());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger(opts =>
            {
                opts.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.BasePath = "/";
                });
            });


            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
