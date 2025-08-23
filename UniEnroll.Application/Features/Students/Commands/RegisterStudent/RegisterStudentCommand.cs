
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Abstractions;
using UniEnroll.Domain.Students;
using UniEnroll.Domain.Students.ValueObjects;

namespace UniEnroll.Application.Features.Students.Commands.RegisterStudent;

public sealed record RegisterStudentCommand(string TenantId, string UserId, string FirstName, string LastName, string Email, string ProgramId, int EntryYear)
    : IRequest<Result<string>>;

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
