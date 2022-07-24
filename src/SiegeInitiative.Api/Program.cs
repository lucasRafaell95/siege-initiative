using SiegeInitiative.Api.Extensions;
using SiegeInitiative.Core.Extensions;
using SiegeInitiative.Infrastructure.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiDependencies(builder.Configuration);

builder.Services.AddCoreDependecies(builder.Configuration);

builder.Services.AddPersistenceDependencies(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHealthCheck();

app.UseAuthorization();

app.MapControllers();

app.Run();