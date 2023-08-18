using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("SqliteStorage");
ArgumentException.ThrowIfNullOrEmpty(connectionString);

builder.Services
    .AddHealthChecks()
    .AddCheck<TimeHealthCheck>("TimeCheck", tags: new[] {"core"})
    .AddSqlite(connectionString, tags: new[] {"infrastructure"});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health", new HealthCheckOptions {Predicate = x => x.Tags.Contains("core")});
app.MapHealthChecks("/health-details", new HealthCheckOptions {Predicate = x => x.Tags.Contains("infrastructure"), ResponseWriter = ResponseWriter.WriteResponse});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();