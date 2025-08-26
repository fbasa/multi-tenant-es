
using FluentValidation;
using MediatR;
using System;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Instructors;

namespace UniEnroll.Application.Features.Instructors.Commands;

public sealed record AssignInstructorToSectionCommand(
    Guid SectionId,
    string InstructorId
) : IRequest<Result<AssignInstructorResult>>;

public sealed class AssignInstructorToSectionCommandValidator : AbstractValidator<AssignInstructorToSectionCommand>
{
    public AssignInstructorToSectionCommandValidator()
    {
        RuleFor(x => x.SectionId).NotEmpty();
        RuleFor(x => x.InstructorId).NotEmpty().MaximumLength(64);
    }
}

public sealed class AssignInstructorToSectionHandler
    : IRequestHandler<AssignInstructorToSectionCommand, Result<AssignInstructorResult>>
{
    private readonly IInstructorCommandRepository _repo;

    public AssignInstructorToSectionHandler(IInstructorCommandRepository repo) => _repo = repo;

    public async Task<Result<AssignInstructorResult>> Handle(AssignInstructorToSectionCommand request, CancellationToken ct)
    {
        var res = await _repo.AssignInstructorToSectionAsync(request.SectionId, request.InstructorId, ct);
        return Result<AssignInstructorResult>.Success(res);
    }
}
