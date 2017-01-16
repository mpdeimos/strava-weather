using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Mpdeimos.StravaWeather.Core
{
	class ExceptionHandler : IExceptionFilter
	{
		private readonly ILogger<ExceptionHandler> logger;

		public ExceptionHandler(ILogger<ExceptionHandler> logger)
		{
			this.logger = logger;
		}

		void IExceptionFilter.OnException(ExceptionContext context)
		{
			var httpException = context.Exception as HttpException;
			if (httpException != null)
			{
				logger.LogInformation($"HttpException: {httpException.StatusCode} {httpException.Message}");
				context.Result = new ContentResult
				{
					Content = httpException.Message,
					StatusCode = (int)httpException.StatusCode
				};
			}
		}
	}
}