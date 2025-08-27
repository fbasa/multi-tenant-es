
using FluentValidation;
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Courses;
using UniEnroll.Domain.Courses;
using UniEnroll.Domain.Courses.ValueObjects;
using UniEnroll.Infrastructure.Common.Abstractions;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Application.Features.Courses.Commands;

public sealed record CreateCourseCommand(CreateCourseRequest Request) : IRequest<Result<string>>;

public sealed class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.Request.TenantId).NotEmpty();
        RuleFor(x => x.Request.Code).NotEmpty().MaximumLength(32);
        RuleFor(x => x.Request.Title).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Request.Units).InclusiveBetween(1, 15);
    }
}

public sealed class CreateCourseHandler(IRepositoryBase<Course> repo, IUnitOfWork uow, IIdGenerator ids) : IRequestHandler<CreateCourseCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateCourseCommand r, CancellationToken ct)
    {
        var id = ids.NewId();
        var course = new Course(id, new CourseCode(r.Request.Code), r.Request.Title, new CreditUnit(r.Request.Units), r.Request.TenantId);
        await repo.AddAsync(course, ct);
        await uow.SaveChangesAsync(ct);
        return Result<string>.Success(id);
    }
}
