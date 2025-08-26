
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Abstractions;
using UniEnroll.Contracts.Sections;
using UniEnroll.Domain.Sections;

namespace UniEnroll.Application.Features.Sections.Queries;

public sealed record ListSectionsQuery(string TenantId, string? CourseId, string? InstructorId, string? TermId, int Page, int Size) : IRequest<Result<PagedResult<SectionDto>>>;

public sealed class ListSectionsHandler : IRequestHandler<ListSectionsQuery, Result<PagedResult<SectionDto>>>
{
    private readonly IQueryRepository<Section> _q;
    public ListSectionsHandler(IQueryRepository<Section> q) => _q = q;

    public async Task<Result<PagedResult<SectionDto>>> Handle(ListSectionsQuery request, CancellationToken ct)
    {
        var filter = await _q.ListAsync(s =>
            (request.CourseId == null || s.CourseId == request.CourseId) &&
            (request.InstructorId == null || s.InstructorId == request.InstructorId) &&
            (request.TermId == null || s.TermId == request.TermId), ct);

        var items = filter.Skip((request.Page - 1) * request.Size).Take(request.Size)
            .Select(s => new SectionDto(s.Id, s.CourseId, s.TermId, s.InstructorId, s.Capacity.Total, s.Capacity.Waitlist, s.Room?.Code, s.MeetingDays, s.StartTime, s.EndTime))
            .ToList();
        var page = new PagedResult<SectionDto>(request.Page, request.Size, filter.Count, items);
        return Result<PagedResult<SectionDto>>.Success(page);
    }
}
