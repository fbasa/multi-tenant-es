
using FluentValidation;
using MediatR;
using System;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Payments;

namespace UniEnroll.Application.Features.Payments.Commands;

public sealed record RefundPaymentCommand(RefundPaymentRequest Request) : IRequest<Result<RefundPaymentResult>>;

public sealed class RefundPaymentCommandValidator : AbstractValidator<RefundPaymentCommand>
{
    public RefundPaymentCommandValidator()
    {
        //RuleFor(x => x.PaymentId).NotEmpty();
        //RuleFor(x => x.Amount).GreaterThan(0);
        //RuleFor(x => x.Reason).NotEmpty().MaximumLength(256);
    }
}

public sealed class RefundPaymentCommandHandler(IPaymentCommandRepository repo) : IRequestHandler<RefundPaymentCommand, Result<RefundPaymentResult>>
{
    public async Task<Result<RefundPaymentResult>> Handle(RefundPaymentCommand r, CancellationToken ct)
        => Result<RefundPaymentResult>.Success(await repo.RefundAsync(r.Request, ct));
}
