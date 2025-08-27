
using FluentValidation;
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Infrastructure.Common.Abstractions;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Application.Features.Enrollment.Commands;

public sealed record EnrollStudentCommand(string TenantId, Guid SectionId, string StudentId, string? Reason) : IRequest<Result<string>>;

public sealed class EnrollStudentCommandValidator : AbstractValidator<EnrollStudentCommand>
{
    public EnrollStudentCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.SectionId).NotEmpty();
        RuleFor(x => x.StudentId).NotEmpty();
    }
}


public sealed class EnrollStudentHandler : IRequestHandler<EnrollStudentCommand, Result<string>>
{
    private readonly IEnrollmentCommandRepository _sql;
    private readonly IIdGenerator _ids;
    public EnrollStudentHandler(IEnrollmentCommandRepository sql, IIdGenerator ids) { _sql = sql; _ids = ids; }

    public async Task<Result<string>> Handle(EnrollStudentCommand request, CancellationToken ct)
    {
        var enrollmentId = _ids.NewId();
        await _sql.EnrollAsync(request.SectionId, request.StudentId, "", ct);
        return Result<string>.Success(enrollmentId);
    }
}
