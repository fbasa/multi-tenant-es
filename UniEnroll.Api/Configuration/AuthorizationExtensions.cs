
using Microsoft.AspNetCore.Authorization;
using UniEnroll.Api.Auth;

namespace UniEnroll.Api.Configuration;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationExtensions(this IServiceCollection services)
    {
        services.AddAuthorization(o =>
        {
            // Student
            o.AddPolicy(Policies.Student.Read, p => p.Requirements.Add(new PermissionRequirement(Policies.Student.Read)));
            o.AddPolicy(Policies.Student.WriteProfile, p => p.Requirements.Add(new PermissionRequirement(Policies.Student.WriteProfile)));
            o.AddPolicy(Policies.Student.Enroll, p => p.Requirements.Add(new PermissionRequirement(Policies.Student.Enroll)));
            o.AddPolicy(Policies.Student.UploadRequirements, p => p.Requirements.Add(new PermissionRequirement(Policies.Student.UploadRequirements)));
            o.AddPolicy(Policies.Student.RequestTranscript, p => p.Requirements.Add(new PermissionRequirement(Policies.Student.RequestTranscript)));
            o.AddPolicy(Policies.Student.ViewTranscript, p => p.Requirements.Add(new PermissionRequirement(Policies.Student.ViewTranscript))); // <-- new
            o.AddPolicy(Policies.Student.ViewLedger, p => p.Requirements.Add(new PermissionRequirement(Policies.Student.ViewLedger)));
            o.AddPolicy(Policies.Student.Pay, p => p.Requirements.Add(new PermissionRequirement(Policies.Student.Pay)));

            // Instructor
            o.AddPolicy(Policies.Instructor.ViewLoads, p => p.Requirements.Add(new PermissionRequirement(Policies.Instructor.ViewLoads)));
            o.AddPolicy(Policies.Instructor.ManageSections, p => p.Requirements.Add(new PermissionRequirement(Policies.Instructor.ManageSections)));
            o.AddPolicy(Policies.Instructor.RecordGrades, p => p.Requirements.Add(new PermissionRequirement(Policies.Instructor.RecordGrades)));

            // Registrar
            o.AddPolicy(Policies.Registrar.FullAccess, p => p.Requirements.Add(new PermissionRequirement(Policies.Registrar.FullAccess)));
            o.AddPolicy(Policies.Registrar.ManageTenants, p => p.Requirements.Add(new PermissionRequirement(Policies.Registrar.ManageTenants)));
            o.AddPolicy(Policies.Registrar.ManageTerms, p => p.Requirements.Add(new PermissionRequirement(Policies.Registrar.ManageTerms)));
            o.AddPolicy(Policies.Registrar.PublishCatalog, p => p.Requirements.Add(new PermissionRequirement(Policies.Registrar.PublishCatalog)));
            o.AddPolicy(Policies.Registrar.ManageCatalog, p => p.Requirements.Add(new PermissionRequirement(Policies.Registrar.ManageCatalog)));
            o.AddPolicy(Policies.Registrar.ManageEnrollmentWindows, p => p.Requirements.Add(new PermissionRequirement(Policies.Registrar.ManageEnrollmentWindows)));
            o.AddPolicy(Policies.Registrar.ManageHolds, p => p.Requirements.Add(new PermissionRequirement(Policies.Registrar.ManageHolds)));
            o.AddPolicy(Policies.Registrar.ApproveGrades, p => p.Requirements.Add(new PermissionRequirement(Policies.Registrar.ApproveGrades)));
            o.AddPolicy(Policies.Registrar.RunGraduationAudit, p => p.Requirements.Add(new PermissionRequirement(Policies.Registrar.RunGraduationAudit)));
            o.AddPolicy(Policies.Registrar.ManageBilling, p => p.Requirements.Add(new PermissionRequirement(Policies.Registrar.ManageBilling)));
            o.AddPolicy(Policies.Registrar.ManageInstructors, p => p.Requirements.Add(new PermissionRequirement(Policies.Registrar.ManageInstructors)));
            o.AddPolicy(Policies.Registrar.OptimizeSchedule, p => p.Requirements.Add(new PermissionRequirement(Policies.Registrar.OptimizeSchedule)));

            // Reporting
            o.AddPolicy(Policies.Reporting.View, p => p.Requirements.Add(new PermissionRequirement(Policies.Reporting.View)));
            o.AddPolicy(Policies.Reporting.Export, p => p.Requirements.Add(new PermissionRequirement(Policies.Reporting.Export)));
        });

        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
        return services;
    }
}
