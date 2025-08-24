
using MediatR;
using System;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Payments.Commands.Common;
using UniEnroll.Contracts.Payments;

namespace UniEnroll.Application.Features.Payments.Commands.CapturePayment;

public sealed record CapturePaymentCommand(
    CapturePaymentRequest Request
) : IRequest<Result<CapturePaymentResult>>;
