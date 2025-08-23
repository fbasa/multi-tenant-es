
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace UniEnroll.Api.Swagger;

public sealed class SwaggerUIOptionsSetup : IConfigureOptions<SwaggerUIOptions>
{
    public void Configure(SwaggerUIOptions options)
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "UniEnroll API v1");
        options.DisplayRequestDuration();
    }
}
