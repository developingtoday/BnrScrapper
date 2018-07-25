using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace RateApi
{
    public class ApplicationInsightsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TelemetryClient _telemetryClient;

        public ApplicationInsightsMiddleware(RequestDelegate next,TelemetryClient telemetryClient)
        {
            _next = next;
            _telemetryClient = telemetryClient;
        }

        public async Task Invoke(HttpContext context)
        {
         
            var bodyRequest = await GetBodyRequest(context);
            
            var response = string.Empty;
            var originalStream = context.Response.Body;
            try
            {
                using (var responseStream = new MemoryStream())
                {
                    context.Response.Body = responseStream;
                    await _next.Invoke(context);
                    response = await GetResponseAsString(context.Response);
                    await responseStream.CopyToAsync(originalStream);
                }

                TrackOperation(context, bodyRequest);
                TrackOperation(context, response);
            }
            catch (Exception)
            {
                
                TrackOperation(context, bodyRequest,500);
                throw;
            }


        }

        private void TrackOperation(HttpContext context, string bodyRequest,int statusCode=200)
        {
            using (var operation =
                _telemetryClient.StartOperation<RequestTelemetry>(context.Request.GetUri().ToString()))
            {
                operation.Telemetry.ResponseCode = statusCode.ToString();
                if (!string.IsNullOrEmpty(bodyRequest))
                {
                    operation.Telemetry.Properties.Add("body", bodyRequest);
                }
            }
        }

        private async Task<string> GetBodyRequest(HttpContext context)
        {
            var request = context.Request;
            if (request == null) return string.Empty;
            request.EnableRewind();
            var sr2 = new StreamReader(request.Body);
            
                var body = await sr2.ReadToEndAsync();
                request.Body.Position = 0L;
                return body;
            
        }

        private async  Task<string> GetResponseAsString(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }
    }
}
