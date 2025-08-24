
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Registrar.Commands.Common;

namespace UniEnroll.Application.Features.Registrar.Commands.UpsertTerm;

public sealed class UpsertTermCommandHandler
    : IRequestHandler<UpsertTermCommand, Result<UpsertTermResult>>
{
    private readonly IRegistrarCommandRepository _repo;
    public UpsertTermCommandHandler(IRegistrarCommandRepository repo) => _repo = repo;

    public async Task<Result<UpsertTermResult>> Handle(UpsertTermCommand r, CancellationToken ct)
        => Result<UpsertTermResult>.Success(await _repo.UpsertTermAsync(r.Request, ct));
}
