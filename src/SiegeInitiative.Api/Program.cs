using SiegeInitiative.Api.Extensions;
using SiegeInitiative.CrossCutting.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiDependencies(builder.Configuration);

builder.Services.RegisterAplicationDependencies(builder.Configuration);

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