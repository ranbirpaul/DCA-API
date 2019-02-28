using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DCAAPISample.Middleware
{
	public class ErrorHandlingMidlleware
	{
		private readonly RequestDelegate next;

		public ErrorHandlingMidlleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context /* other dependencies */)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			var code = HttpStatusCode.InternalServerError; // 500 if unexpected

			
			var result = JsonConvert.SerializeObject(new { Message = exception.Message, StatusCode = code });
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)code;
			return context.Response.WriteAsync(result);
		}
	}
	public class Error
	{
		public string ErrorCode { get; set; }
		public string ErrorMessage { get; set; }
	}
}
