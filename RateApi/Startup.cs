using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BnrScrapperLogic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
         
            services.AddSingleton<TelemetryClient>(new TelemetryClient());
            services.AddTransient<IRateRepository>(x =>
                new DapperRateRepository(Environment.GetEnvironmentVariable("DatabaseConnectionString")));
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
            app.UseMiddleware<ApplicationInsightsMiddleware>();
            app.UseMvc();
            
            app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling =
                    PropertyNameHandling.CamelCase;
            });
           



        }

    }


    public class AiFilter : IActionFilter
    {
        private readonly TelemetryClient _clientAi;

        public AiFilter(TelemetryClient telemetryClient)
        {
            this._clientAi = telemetryClient;
        }


        public void OnActionExecuting(ActionExecutingContext context)
        {
            using (var operation =
                _clientAi.StartOperation<RequestTelemetry>(context.HttpContext.Request.GetUri().ToString()))
            {
                var request = context.HttpContext.Request;
                if (request != null)
                {
                    request.EnableRewind();
                    var sr2 = new StreamReader(request.Body);
                    var body = sr2.ReadToEnd();
                    if (!string.IsNullOrEmpty(body))
                    {
                        operation.Telemetry.Properties.Add("body", body);
                    }
                    request.Body.Position = 0L;
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //var str = (context.Result as JsonResult)?.Value;
            //if (string.IsNullOrEmpty(str))
            //{
            //    str=(context.)
            //}

            //using (var operation =
            //    _clientAi.StartOperation<RequestTelemetry>(context.HttpContext.Request.GetUri().ToString()))
            //{
            //    var request = context.HttpContext.Request;
            //    if (request != null)
            //    {
            //        var body = str.Value.ToString();
            //        if (!string.IsNullOrEmpty(body))
            //        {
            //            operation.Telemetry.Properties.Add("result", body);
            //        }
            //        request.Body.Position = 0L;
            //    }
            //}




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

}




