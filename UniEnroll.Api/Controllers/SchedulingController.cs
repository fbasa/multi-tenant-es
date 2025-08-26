using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Application.Features.Scheduling.Commands;
using UniEnroll.Application.Features.Scheduling.Queries;
using UniEnroll.Contracts.Scheduling;

namespace UniEnroll.Api.Controllers;

public sealed class SchedulingController : BaseApiController
{
    public SchedulingController(ISender sender) : base(sender) { }

    //[HttpPost("{tenantId}/timetable/build")]
    //public async Task<IActionResult> Build([FromRoute] string tenantId, [FromBody] BuildTimetableCommand body, CancellationToken ct)
    //    => Ok(await Sender.Send(body with { TenantId = tenantId }, ct));

    //[HttpPost("{tenantId}/sections/{sectionId}/room")]
    //public async Task<IActionResult> AssignRoom([FromRoute] string tenantId, [FromRoute] string sectionId, [FromBody] AssignRoomCommand body, CancellationToken ct)
    //    => Ok(await Sender.Send(body with { TenantId = tenantId, SectionId = sectionId }, ct));

    //[HttpPost("{tenantId}/optimize")]
    //public async Task<IActionResult> Optimize([FromRoute] string tenantId, [FromBody] OptimizeScheduleCommand body, CancellationToken ct)
    //    => Ok(await Sender.Send(body with { TenantId = tenantId }, ct));

    //[HttpGet("{tenantId}/students/{studentId}/schedule")]
    //[ProducesResponseType(typeof(TimetableDto), StatusCodes.Status200OK)]
    //public async Task<IActionResult> GetStudentSchedule([FromRoute] string tenantId, [FromRoute] string studentId, [FromQuery] string termId, CancellationToken ct)
    //    => Ok((await Sender.Send(new GetStudentScheduleQuery(tenantId, studentId, termId), ct)).Value);

    //[HttpGet("{tenantId}/rooms/conflicts")]
    //[ProducesResponseType(typeof(IReadOnlyList<RoomConflictDto>), StatusCodes.Status200OK)]
    //public async Task<IActionResult> ListRoomConflicts([FromRoute] string tenantId, [FromQuery] string termId, CancellationToken ct)
    //    => Ok((await Sender.Send(new ListRoomConflictsQuery(tenantId, termId), ct)).Value);
}
