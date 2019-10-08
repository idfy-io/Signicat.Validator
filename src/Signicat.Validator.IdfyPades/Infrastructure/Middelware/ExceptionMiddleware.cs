using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;
using Serilog.Events;
using Signicat.Validator.IdfyPades.Infrastructure.Extensions;
using Signicat.Validator.IdfyPades.Models;
using Signicat.Validator.IdfyPades.Models.Errors;

namespace Signicat.Validator.IdfyPades.Infrastructure.Middelware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger = Log.Logger.ForContext<ExceptionMiddleware>();
        private readonly Random _random = new Random();

        private const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0} ms";

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;            
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var requestId = GenerateRequestId();

            var logLevel = LogEventLevel.Information;
            var logProps = new List<ILogEventEnricher>()
            {
                new PropertyEnricher("RequestId", requestId),
                new PropertyEnricher("RequestIpAddress", httpContext.Request.IpAddress()),
            };
            

            httpContext.Response.OnStarting(state =>
            {
                var ctx = (HttpContext)state;
                ctx.Response.Headers.Add("RequestId", requestId);
                return Task.CompletedTask;
            }, httpContext);

            using (LogContext.Push(logProps.ToArray()))
            {
                var sw = Stopwatch.StartNew();
                Exception ex = null;

                try
                {
                    await _next.Invoke(httpContext);
                    logLevel = httpContext.Response.StatusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;
                }
                catch (Exception e)
                {
                    httpContext.Response.ContentType = "application/json";

                    if (httpContext.RequestAborted.IsCancellationRequested)
                    {
                        httpContext.Response.StatusCode = 499;
                        await httpContext.Response.WriteAsync("");
                    }
                    else
                    {
                        ex = e;
                        logLevel = LogEventLevel.Error;
                        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        SignicatError error;

                        if (e is SignicatException idfyIdException)
                        {
                            httpContext.Response.StatusCode = GetResponseStatusCode(idfyIdException);
                            error = idfyIdException.Error;
                        }
                        else
                        {
                            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            error = new SignicatError(SignicatErrorCode.UnexpectedError);
                        }

                        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(
                            new ApiErrorObject(error.Message, error.Code),
                            _serializerSettings));
                    }
                }
                finally
                {
                    sw.Stop();

                    if (!(httpContext.Request.Method == "OPTIONS" && httpContext.Response.StatusCode < 400))
                    {
                        _logger.Write(logLevel, ex, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path,
                            httpContext.Response.StatusCode, sw.Elapsed.TotalMilliseconds);
                    }
                }
            }
        }

        private string GenerateRequestId()
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 13)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        private int GetResponseStatusCode(SignicatException e)
        {
            var type = e.GetType();
            if (_exceptionStatusCodes.ContainsKey(type))
                return (int)_exceptionStatusCodes[type];

            return (int)HttpStatusCode.BadRequest;
        }

        private readonly Dictionary<Type, HttpStatusCode> _exceptionStatusCodes = new Dictionary<Type, HttpStatusCode>()
        {
            //{typeof(InvalidSessionOperationException), HttpStatusCode.BadRequest}
        };
    }
}