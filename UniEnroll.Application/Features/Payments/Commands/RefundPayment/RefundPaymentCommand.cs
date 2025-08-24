
using System;
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Payments.Commands.Common;
using UniEnroll.Contracts.Payments;

namespace UniEnroll.Application.Features.Payments.Commands.RefundPayment;

public sealed record RefundPaymentCommand(
    RefundPaymentRequest Request
) : IRequest<Result<RefundPaymentResult>>;
