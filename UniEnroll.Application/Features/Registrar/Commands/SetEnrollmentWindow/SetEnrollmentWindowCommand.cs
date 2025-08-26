
using FluentValidation;
using MediatR;
using System;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Registrar;

namespace UniEnroll.Application.Features.Registrar.Commands;

public sealed record SetEnrollmentWindowCommand(SetEnrollmentWindowRequest Request) : IRequest<Result<SetEnrollmentWindowResult>>;

public sealed class SetEnrollmentWindowCommandValidator : AbstractValidator<SetEnrollmentWindowCommand>
{
    public SetEnrollmentWindowCommandValidator()
    {
        //RuleFor(x => x.TermId).NotEmpty();
        //RuleFor(x => x.StartAt).LessThan(x => x.EndAt);
    }
}

public sealed class SetEnrollmentWindowCommandHandler
    : IRequestHandler<SetEnrollmentWindowCommand, Result<SetEnrollmentWindowResult>>
{
    private readonly IRegistrarCommandRepository _repo;
    public SetEnrollmentWindowCommandHandler(IRegistrarCommandRepository repo) => _repo = repo;

    public async Task<Result<SetEnrollmentWindowResult>> Handle(SetEnrollmentWindowCommand r, CancellationToken ct)
        => Result<SetEnrollmentWindowResult>.Success(await _repo.SetEnrollmentWindowAsync(r.Request, ct));
}
