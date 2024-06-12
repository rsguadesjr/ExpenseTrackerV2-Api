using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Transactions.Common;
using ExpenseTracker.Domain.Models.Common;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System.Reflection;

namespace ExpenseTracker.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var errors = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(failure => failure != null)
                .Select(failure => Error.Validation(failure.PropertyName, failure.ErrorMessage))
                .ToList();

            if (errors.Count != 0)
            {
                var validationErrors = typeof(Result<>)
                    .GetGenericTypeDefinition()
                    .MakeGenericType(typeof(TResponse).GenericTypeArguments[0])!
                    .GetMethod("Failure", new[] { typeof(List<Error>) })!.Invoke(null, new object[] { errors });

                return (TResponse)validationErrors!;
            }

            return await next();
        }
    }
}
