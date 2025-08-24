
using System;
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Payments.Queries.Common;

namespace UniEnroll.Application.Features.Payments.Queries.GetPaymentStatus;

public sealed record GetPaymentStatusQuery(Guid PaymentId) : IRequest<Result<PaymentStatusResult?>>;
