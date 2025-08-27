
using FluentValidation;
using MediatR;
using System;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Scheduling;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Application.Features.Scheduling.Commands;

public sealed record BuildTimetableCommand(string StudentId, Guid TermId) : IRequest<Result<BuildTimetableResult>>;

public sealed class BuildTimetableCommandValidator : AbstractValidator<BuildTimetableCommand>
{
    public BuildTimetableCommandValidator()
    {
        RuleFor(x => x.StudentId).NotEmpty().MaximumLength(64);
        RuleFor(x => x.TermId).NotEmpty();
    }
}

public sealed class BuildTimetableCommandHandler : IRequestHandler<BuildTimetableCommand, Result<BuildTimetableResult>>
{
    private readonly ISchedulingRepository _repo;
    public BuildTimetableCommandHandler(ISchedulingRepository repo) => _repo = repo;

    public async Task<Result<BuildTimetableResult>> Handle(BuildTimetableCommand request, CancellationToken ct)
        => Result<BuildTimetableResult>.Success(await _repo.BuildTimetableAsync(request.StudentId, request.TermId, ct));
}

