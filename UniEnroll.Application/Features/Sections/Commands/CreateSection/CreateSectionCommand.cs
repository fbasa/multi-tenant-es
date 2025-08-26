
using System;
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Abstractions;
using UniEnroll.Domain.Sections;
using UniEnroll.Domain.Sections.ValueObjects;

namespace UniEnroll.Application.Features.Sections.Commands;

public sealed record CreateSectionCommand(
    string TenantId,
    string CourseId,
    string TermId,
    string InstructorId,
    int Capacity,
    int WaitlistCapacity,
    DayOfWeek[] Days,
    TimeSpan Start,
    TimeSpan End)
 : IRequest<Result<string>>;

public sealed class CreateSectionHandler : IRequestHandler<CreateSectionCommand, Result<string>>
{
    private readonly IRepository<Section> _repo;
    private readonly IUnitOfWork _uow;
    private readonly IIdGenerator _ids;
    public CreateSectionHandler(IRepository<Section> repo, IUnitOfWork uow, IIdGenerator ids) { _repo = repo; _uow = uow; _ids = ids; }
    public async Task<Result<string>> Handle(CreateSectionCommand request, CancellationToken ct)
    {
        var id = _ids.NewId();
        var sec = new Section(id, request.CourseId, request.TermId, request.InstructorId, new Capacity(request.Capacity, request.WaitlistCapacity), request.Days, request.Start, request.End, request.TenantId);
        await _repo.AddAsync(sec, ct);
        await _uow.SaveChangesAsync(ct);
        return Result<string>.Success(id);
    }
}
