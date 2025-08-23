using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using UniEnroll.Infrastructure.Common.Options;

namespace UniEnroll.Api.Security;

public static class JwtAuthExtensions
{
    public static void AddJwt(IServiceCollection services, IConfiguration config)
    {
        var jwt = new JwtOptions();
        config.GetSection("Jwt").Bind(jwt);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(string.IsNullOrWhiteSpace(jwt.SigningKey) ? "dev-signing-key-change-me" : jwt.SigningKey));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, ValidIssuer = jwt.Issuer,
                    ValidateAudience = true, ValidAudience = jwt.Audience,
                    ValidateIssuerSigningKey = true, IssuerSigningKey = key,
                    ValidateLifetime = true
                };
            });

        services.AddAuthorization();
    }
}
