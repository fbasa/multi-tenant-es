
using System;
using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Application.Features.Registrar.Commands.Common;
using UniEnroll.Contracts.Registrar;

namespace UniEnroll.Application.Abstractions;

public interface IRegistrarCommandRepository
{
    Task<UpsertTermResult> UpsertTermAsync(UpsertTermRequest Request, CancellationToken ct);
    Task<SetEnrollmentWindowResult> SetEnrollmentWindowAsync(SetEnrollmentWindowRequest request, CancellationToken ct);
}
