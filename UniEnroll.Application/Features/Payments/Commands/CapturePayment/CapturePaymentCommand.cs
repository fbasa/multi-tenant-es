
using FluentValidation;
using MediatR;
using System;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Payments;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Application.Features.Payments.Commands;

public sealed record CapturePaymentCommand(CapturePaymentRequest Request) : IRequest<Result<CapturePaymentResult>>;

public sealed class CapturePaymentCommandValidator : AbstractValidator<CapturePaymentCommand>
{
    public CapturePaymentCommandValidator()
    {
        //RuleFor(x => x.InvoiceId).NotEmpty();
        //RuleFor(x => x.Amount).GreaterThan(0);
        //RuleFor(x => x.Currency).NotEmpty().Length(3);
        //RuleFor(x => x.Method).NotEmpty();
    }
}

public sealed class CapturePaymentCommandHandler : IRequestHandler<CapturePaymentCommand, Result<CapturePaymentResult>>
{
    private readonly IPaymentCommandRepository _repo;
    public CapturePaymentCommandHandler(IPaymentCommandRepository repo) => _repo = repo;

    public async Task<Result<CapturePaymentResult>> Handle(CapturePaymentCommand r, CancellationToken ct)
        => Result<CapturePaymentResult>.Success(await _repo.CaptureAsync(r.Request, ct));
}
