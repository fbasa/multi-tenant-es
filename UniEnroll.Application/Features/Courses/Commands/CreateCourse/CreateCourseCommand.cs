
using FluentValidation;
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Domain.Courses;
using UniEnroll.Domain.Courses.ValueObjects;
using UniEnroll.Infrastructure.Common.Abstractions;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Application.Features.Courses.Commands;

public sealed record CreateCourseCommand(string TenantId, string Code, string Title, int Units) : IRequest<Result<string>>;

public sealed class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty().MaximumLength(32);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Units).InclusiveBetween(1, 15);
    }
}

public sealed class CreateCourseHandler(IRepositoryBase<Course> repo, IUnitOfWork uow, IIdGenerator ids) : IRequestHandler<CreateCourseCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateCourseCommand request, CancellationToken ct)
    {
        var id = ids.NewId();
        var course = new Course(id, new CourseCode(request.Code), request.Title, new CreditUnit(request.Units), request.TenantId);
        await repo.AddAsync(course, ct);
        await uow.SaveChangesAsync(ct);
        return Result<string>.Success(id);
    }
}
