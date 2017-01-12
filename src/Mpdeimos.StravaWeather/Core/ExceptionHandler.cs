using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mpdeimos.StravaWeather.Core
{
	class ExceptionHandler : IExceptionFilter
	{
		void IExceptionFilter.OnException(ExceptionContext context)
		{
			var httpException = context.Exception as HttpException;
			if (httpException != null)
			{
				context.Result = new ContentResult
				{
					Content = httpException.Message,
					StatusCode = (int)httpException.StatusCode
				};
			}
		}
	}
}