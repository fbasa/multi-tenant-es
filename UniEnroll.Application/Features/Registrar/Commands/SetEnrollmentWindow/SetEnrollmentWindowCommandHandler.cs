
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Registrar.Commands.Common;

namespace UniEnroll.Application.Features.Registrar.Commands.SetEnrollmentWindow;

public sealed class SetEnrollmentWindowCommandHandler
    : IRequestHandler<SetEnrollmentWindowCommand, Result<SetEnrollmentWindowResult>>
{
    private readonly IRegistrarCommandRepository _repo;
    public SetEnrollmentWindowCommandHandler(IRegistrarCommandRepository repo) => _repo = repo;

    public async Task<Result<SetEnrollmentWindowResult>> Handle(SetEnrollmentWindowCommand r, CancellationToken ct)
        => Result<SetEnrollmentWindowResult>.Success(await _repo.SetEnrollmentWindowAsync(r.Request, ct));
}
