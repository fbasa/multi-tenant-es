
using FluentValidation;
using MediatR;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Domain.Students;
using UniEnroll.Domain.Students.ValueObjects;

namespace UniEnroll.Application.Features.Students.Commands;

public sealed record RegisterStudentCommand(string TenantId, string UserId, string FirstName, string LastName, string Email, string ProgramId, int EntryYear)
    : IRequest<Result<string>>;

public sealed class RegisterStudentCommandValidator : AbstractValidator<RegisterStudentCommand>
{
    public RegisterStudentCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.ProgramId).NotEmpty();
        RuleFor(x => x.EntryYear).InclusiveBetween(1900, 2100);
    }
}


public sealed class RegisterStudentHandler : IRequestHandler<RegisterStudentCommand, Result<string>>
{
    private readonly IRepository<Student> _repo;
    private readonly IUnitOfWork _uow;
    private readonly IIdGenerator _ids;
    public RegisterStudentHandler(IRepository<Student> repo, IUnitOfWork uow, IIdGenerator ids) { _repo = repo; _uow = uow; _ids = ids; }

    public async Task<Result<string>> Handle(RegisterStudentCommand request, CancellationToken ct)
    {
        var id = _ids.NewId();
        var student = new Student(id, new StudentName(request.FirstName, request.LastName), request.Email, request.ProgramId, request.EntryYear, request.TenantId);
        await _repo.AddAsync(student, ct);
        await _uow.SaveChangesAsync(ct);
        return Result<string>.Success(id);
    }
}
