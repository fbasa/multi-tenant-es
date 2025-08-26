
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Billing;
using UniEnroll.Contracts.Common;

namespace UniEnroll.Application.Features.Billing.Queries;

public sealed record GetLedgerQuery(string TenantId, string StudentId) : IRequest<Result<IReadOnlyList<InvoiceDto>>>;

public sealed class GetLedgerHandler : IRequestHandler<GetLedgerQuery, Result<IReadOnlyList<InvoiceDto>>>
{
    public Task<Result<IReadOnlyList<InvoiceDto>>> Handle(GetLedgerQuery request, CancellationToken ct)
    {
        var list = new List<InvoiceDto>();
        return Task.FromResult(Result<IReadOnlyList<InvoiceDto>>.Success(list));
    }
}
