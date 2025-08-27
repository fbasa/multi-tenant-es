
using FluentValidation;
using MediatR;
using System;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Registrar;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Application.Features.Registrar.Commands;

public sealed record UpsertTermCommand(UpsertTermRequest Request) : IRequest<Result<UpsertTermResult>>;

public sealed class UpsertTermCommandValidator : AbstractValidator<UpsertTermCommand>
{
    public UpsertTermCommandValidator()
    {
        //RuleFor(x => x.TermId).NotEmpty();
        //RuleFor(x => x.Code).NotEmpty().MaximumLength(32);
        //RuleFor(x => x.Name).NotEmpty().MaximumLength(128);
        //RuleFor(x => x.StartDate).LessThan(x => x.EndDate);
    }
}

public sealed class UpsertTermCommandHandler
    : IRequestHandler<UpsertTermCommand, Result<UpsertTermResult>>
{
    private readonly IRegistrarCommandRepository _repo;
    public UpsertTermCommandHandler(IRegistrarCommandRepository repo) => _repo = repo;

    public async Task<Result<UpsertTermResult>> Handle(UpsertTermCommand r, CancellationToken ct)
        => Result<UpsertTermResult>.Success(await _repo.UpsertTermAsync(r.Request, ct));
}

