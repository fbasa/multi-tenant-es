
using System;

namespace UniEnroll.Contracts.Registrar;

public sealed record SetEnrollmentWindowRequest(string TermId, DateTimeOffset OpensAt, DateTimeOffset ClosesAt);
