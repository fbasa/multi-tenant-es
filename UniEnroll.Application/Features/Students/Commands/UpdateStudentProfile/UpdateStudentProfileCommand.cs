
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Abstractions;
using UniEnroll.Domain.Students;
using UniEnroll.Domain.Students.ValueObjects;

namespace UniEnroll.Application.Features.Students.Commands;

public sealed record UpdateStudentProfileCommand(string TenantId, string StudentId, string FirstName, string LastName, string Email, string ProgramId, int EntryYear)
    : IRequest<Result<bool>>;

public sealed class UpdateStudentProfileHandler : IRequestHandler<UpdateStudentProfileCommand, Result<bool>>
{
    private readonly IRepository<Student> _repo;
    private readonly IUnitOfWork _uow;
    public UpdateStudentProfileHandler(IRepository<Student> repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<Result<bool>> Handle(UpdateStudentProfileCommand request, CancellationToken ct)
    {
        var student = await _repo.GetAsync(s => s.Id == request.StudentId, ct);
        if (student is null) return Result<bool>.Failure("Not found");
        student.GetType().GetProperty("Name")?.SetValue(student, new StudentName(request.FirstName, request.LastName));
        student.GetType().GetProperty("Email")?.SetValue(student, request.Email);
        student.GetType().GetProperty("ProgramId")?.SetValue(student, request.ProgramId);
        student.GetType().GetProperty("EntryYear")?.SetValue(student, request.EntryYear);
        await _uow.SaveChangesAsync(ct);
        return Result<bool>.Success(true);
    }
}
