using ErrorOr;
using FluentValidation;
using MediatR;

namespace Prover.Entrevista.Core.Common.Behaviors;

public class ValidationBehaviors<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validator is null)
            return await next();

        var resultValidator = await _validator.ValidateAsync(request, cancellationToken);
        if (resultValidator.IsValid) 
            return await next();

        var errors = resultValidator.Errors
            .ConvertAll(e => Error.Validation(e.ErrorCode, e.ErrorMessage))
            .ToList();

        return (dynamic)errors;
    }
}