
using System;

namespace UniEnroll.Contracts.Registrar;

public sealed record SetEnrollmentWindowRequest(
       Guid TermId,
        DateTimeOffset StartAt,
        DateTimeOffset EndAt,
        byte[]? RowVersion
    );
