
using System;
using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Contracts.Registrar;

namespace UniEnroll.Infrastructure.EF.Repositories.Contracts;

public interface IRegistrarCommandRepository
{
    Task<UpsertTermResult> UpsertTermAsync(UpsertTermRequest Request, CancellationToken ct);
    Task<SetEnrollmentWindowResult> SetEnrollmentWindowAsync(SetEnrollmentWindowRequest request, CancellationToken ct);
}
