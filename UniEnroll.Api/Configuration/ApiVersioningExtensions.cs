
using Asp.Versioning;

namespace UniEnroll.Api.Configuration;

public static class ApiVersioningExtensions
{
    public static IServiceCollection AddApiVersioningV1(this IServiceCollection services)
    {
        services
            .AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1);        //< --major - only, yields v1
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
                o.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";   // <-- shows v1 (not v1.0)
                o.SubstituteApiVersionInUrl = true;
            });

        return services;
    }
}
