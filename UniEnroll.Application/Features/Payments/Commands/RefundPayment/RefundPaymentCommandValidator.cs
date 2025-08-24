
using FluentValidation;

namespace UniEnroll.Application.Features.Payments.Commands.RefundPayment;

public sealed class RefundPaymentCommandValidator : AbstractValidator<RefundPaymentCommand>
{
    public RefundPaymentCommandValidator()
    {
        //RuleFor(x => x.PaymentId).NotEmpty();
        //RuleFor(x => x.Amount).GreaterThan(0);
        //RuleFor(x => x.Reason).NotEmpty().MaximumLength(256);
    }
}
