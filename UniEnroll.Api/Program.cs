using UniEnroll.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiCore(builder.Configuration);

var app = builder.Build();

app.UseApiCore(app.Environment.IsDevelopment());

app.MapControllers();

app.Run();

