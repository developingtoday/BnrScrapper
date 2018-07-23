using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NJsonSchema;
using NSwag.AspNetCore;

namespace RateApi
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
            services.AddMvc();

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
                app.UseExceptionHandler();
            }


            var aiconfiguration = app.ApplicationServices
                .GetService<Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration>();
           
            var config = new ApplicationInsightConfig()
            {
                RoleName = Configuration["ApplicationInsights:ApplicationName"],
                DisableContext = bool.Parse(Configuration["ApplicationInsights:DisableInstrumentation"]),
                InstrumentationKey = Environment.GetEnvironmentVariable("AiInstrumentationKey")
            };
            aiconfiguration.DisableTelemetry = bool.Parse(Configuration["ApplicationInsights:DisableInstrumentation"]);
            aiconfiguration.TelemetryInitializers.Add(new AiTelemetryInitializer(config));
            app.UseMvc();
            app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling =
                    PropertyNameHandling.CamelCase;
            });

            app.Use(async (context, next) => { await next.Invoke();});
            app.Run(async context =>
            {

            });
        }

      
    }

    public class ApplicationInsightConfig
    {
        public string RoleName { get; set; }
        public bool DisableContext { get; set; }

        public string InstrumentationKey { get; set; }

    }

    public class AiTelemetryInitializer : ITelemetryInitializer
    {
        private readonly ApplicationInsightConfig _config;

        public AiTelemetryInitializer(ApplicationInsightConfig config)
        {
            _config = config;
        }

        public void Initialize(ITelemetry telemetry)
        {

            telemetry.Context.Cloud.RoleName = _config.RoleName;

        }
    }

    public class RequestBodyInitializer : ITelemetryInitializer
    {
        readonly IHttpContextAccessor httpContextAccessor;

        public RequestBodyInitializer(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public void Initialize(ITelemetry telemetry)
        {
            if (!(telemetry is RequestTelemetry requestTelemetry)) return;
            if (!httpContextAccessor.HttpContext.Request.Body.CanRead) return;
            //Allows re-usage of the stream
            httpContextAccessor.HttpContext.Request.EnableRewind();

            var stream = new StreamReader(httpContextAccessor.HttpContext.Request.Body);
            var body = stream.ReadToEnd();

            //Reset the stream so data is not lost
            httpContextAccessor.HttpContext.Request.Body.Position = 0;
            requestTelemetry.Properties.Add("body", body);
        }
    }
}




