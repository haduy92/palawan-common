using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog;

namespace Palawan.Common.Middlewares
{
	public class EnrichLogContextMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger _logger;

		public EnrichLogContextMiddleware(ILogger logger, RequestDelegate next)
		{
			_logger = logger;
			_next = next;
		}

		public Task Invoke(HttpContext context)
		{
			_logger.Information("[Request] {Method} {Uri}.",
					context.Request.Method,
					context.Request.GetDisplayUrl(),
					context.Request.Headers["UserId"]);

			return _next(context);
		}
	}
}