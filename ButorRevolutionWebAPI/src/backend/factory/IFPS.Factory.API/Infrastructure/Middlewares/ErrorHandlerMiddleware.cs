using ENCO.DDD.Repositories;
using IFPS.Factory.Application.Exceptions;
using IFPS.Factory.Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Infrastructure.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlerMiddleware> logger;
        private readonly IHostingEnvironment hostingEnvironment;

        public ErrorHandlerMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlerMiddleware> logger,
            IHostingEnvironment hostingEnvironment)
        {
            this.next = next;
            this.logger = logger;
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Call the next delegate/middleware in the pipeline
                await next(context);
            }
            catch (EntityNotFoundException e)
            {
                logger.LogError(e, "Entity not found exception");

                await WriteAsJsonAsync(
                    context,
                    (int)HttpStatusCode.NotFound,
                    new ErrorDto
                    {
                        Message = e.Message,
                        StackTrace = e.StackTrace
                    });
            }
            catch (IFPSDomainException e)
            {
                logger.LogError(e, "Unhandled exception catched.");

                await WriteAsJsonAsync(
                    context,
                    (int)HttpStatusCode.BadRequest,
                    new ErrorDto
                    {
                        Message = e.Message,
                        StackTrace = e.StackTrace
                    });
            }
            catch (IFPSValidationAppException e)
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var json = SerializeObject(e.Errors);
                await context.Response.WriteAsync(json);
            }
            catch (IFPSAppException e)
            {
                logger.LogError(e, "App exception catched.");

                await WriteAsJsonAsync(
                    context,
                    e.ErrorCode.HasValue
                        ? (int)e.ErrorCode
                        : (int)HttpStatusCode.BadRequest,
                    new ErrorDto
                    {
                        Message = e.Message,
                        StackTrace = e.StackTrace
                    });
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unhandled exception catched.");

                await WriteAsJsonAsync(
                    context,
                    (int)HttpStatusCode.InternalServerError,
                    new ErrorDto
                    {
                        StackTrace = e.StackTrace
                    });
            }

        }

        private Task WriteAsJsonAsync(HttpContext context, int statusCode, ErrorDto payload)
        {
            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var json = hostingEnvironment.IsDevelopment()
                ? string.IsNullOrWhiteSpace(payload.Message)
                    ? SerializeObject(new { payload.StackTrace })
                    : SerializeObject(payload)
                : string.IsNullOrWhiteSpace(payload.Message)
                    ? ""
                    : SerializeObject(new { payload.Message });

            return context.Response.WriteAsync(json);
        }

        private string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(
                obj,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }

        class ErrorDto
        {
            public string Message { get; set; }

            public string StackTrace { get; set; }
        }
    }
}
