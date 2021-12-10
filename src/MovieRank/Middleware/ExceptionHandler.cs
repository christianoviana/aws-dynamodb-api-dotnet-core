using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MovieRank.Api.Middleware
{
    /// <summary>
    /// Global handler exeptions in application
    /// </summary>
    public class ExceptionHandler
    {
        private ILogger<ExceptionHandler> _logger;
        private RequestDelegate _next;


        public ExceptionHandler(ILogger<ExceptionHandler> logger, RequestDelegate next)
        {
            this._logger = logger;
            this._next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                this._logger.LogInformation($"Method: {context.Request.Method} - {context.Request.Scheme}://{context.Request.Host}/{context.Request.Path}/{context.Request.Query.ToString()}");
                this._logger.LogError($"Erro: {ex.ToString()}");

                var response = new { StatusCode = HttpStatusCode.InternalServerError, Message = ex.Message};

                await context.Response.WriteAsJsonAsync(response); 
            }
        }
    }
}
