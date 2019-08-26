using System.Collections.Generic;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using QandA.Features.Users;

namespace QandA.Features
{
	public static class ValidationExceptionExtensions
	{
		public static ProblemDetailsException ThrowValidationException(this object here, string property, string error)
		{
			var validationProblemDetails = new ValidationProblemDetails(new Dictionary<string, string[]> {{property, new[] {error}}})
				{
					Status = 400
				};
			var problemDetails = new ProblemDetailsException(validationProblemDetails);
			throw problemDetails;
		}
	}
}