
using FluentValidation;

namespace UniEnroll.Application.Features.Payments.Commands.CapturePayment;

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
