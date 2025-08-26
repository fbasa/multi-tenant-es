
using FluentValidation;
using MediatR;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Instructors;

namespace UniEnroll.Application.Features.Instructors.Commands;

public sealed record UpsertInstructorCommand(
    string InstructorId,
    string FirstName,
    string LastName,
    string Email
) : IRequest<Result<UpsertInstructorResult>>;

public sealed class UpsertInstructorCommandValidator : AbstractValidator<UpsertInstructorCommand>
{
    public UpsertInstructorCommandValidator()
    {
        RuleFor(x => x.InstructorId).NotEmpty().MaximumLength(64);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
    }
}
public sealed class UpsertInstructorCommandHandler
    : IRequestHandler<UpsertInstructorCommand, Result<UpsertInstructorResult>>
{
    private readonly IInstructorCommandRepository _repo;

    public UpsertInstructorCommandHandler(IInstructorCommandRepository repo) => _repo = repo;

    public async Task<Result<UpsertInstructorResult>> Handle(UpsertInstructorCommand request, CancellationToken ct)
    {
        var res = await _repo.UpsertInstructorAsync(request.InstructorId, request.FirstName, request.LastName, request.Email, ct);
        return Result<UpsertInstructorResult>.Success(res);
    }
}
